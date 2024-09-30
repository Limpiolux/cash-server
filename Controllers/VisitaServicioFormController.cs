using cash_server.Data;
using cash_server.Models;
using cash_server.Servicios;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Document = iTextSharp.text.Document;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace cash_server.Controllers
{
    [RoutePrefix("visitaservicioform")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VisitaServicioFormController:ApiController
    {
        private readonly VisitaServicioFormData _visitaServicioFormData;
        private readonly VisitaServicioData _visitaServicioData;
        private readonly UnidadNegocioData _unidadNegocioData;
        private readonly EmailService _emailService;
        private readonly FormularioData _formularioData;


        public VisitaServicioFormController()
        {
            _visitaServicioFormData = new VisitaServicioFormData();
            _visitaServicioData = new VisitaServicioData();
            _unidadNegocioData = new UnidadNegocioData();
            _emailService = new EmailService();
            _formularioData = new FormularioData();
        }
        /*datos a enviar
        [
        {
            "VisitaId": 7,
            "FormId": 3,
            "Imagen1": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAAEAYABgAA,
            "Imagen2": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAAEAYABgAA,
            "Imagen3": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAAEAYABgAA,
            "Item": "Ejemplo de item 1",
            "SubItem": "Ejemplo de subitem 1",
            "Comentario": "Ejemplo de comentario 1",
            "Respuesta": "Ejemplo de respuesta 1",
            "Version": 16,
            "ComentarioGeneral": "Ejemplo"
        },
        {
            "VisitaId": 7,
            "FormId": 2,
            "Imagen1": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAAEAYABgAA,
            "Imagen2": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAAEAYABgAA,
            "Imagen3": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAAEAYABgAA,
            "Item": "Ejemplo de item 2",
            "SubItem": "Ejemplo de subitem 2",
            "Comentario": null,
            "Respuesta": "Ejemplo de respuesta 2",
            "Version": 16,
            "ComentarioGeneral": "Ejemplo"
        }
  
        ]
        //el campo comentario puede ser null, puede estar completo o no,
        si el usuario no completo comentario en el formulario, por default se le coloca null
        */
        [HttpPost]
        [Route("crear")]
        public IHttpActionResult CrearVisitaServicioForm([FromBody] List<VisitaServicioForm> visitasServicioForm)
        {
            try
            {
                if (visitasServicioForm == null || visitasServicioForm.Count == 0)
                {
                    return BadRequest("La lista de visitas de servicio está vacía o nula.");
                }

                foreach (var visitaServicioForm in visitasServicioForm)
                {
                    //antes de guardar proceso las imagenes
                    if (!string.IsNullOrEmpty(visitaServicioForm.Imagen1))
                    {
                        //guardarimagen guarda la imagen fisicamente y dvuelve la ruta
                        visitaServicioForm.Imagen1 = GuardarImagen(visitaServicioForm.Imagen1, visitaServicioForm.VisitaId, "Imagen1");
                    }

                    if (!string.IsNullOrEmpty(visitaServicioForm.Imagen2))  
                    {
                        visitaServicioForm.Imagen2 = GuardarImagen(visitaServicioForm.Imagen2, visitaServicioForm.VisitaId, "Imagen2");
                    }

                    if (!string.IsNullOrEmpty(visitaServicioForm.Imagen3))
                    {
                        visitaServicioForm.Imagen3 = GuardarImagen(visitaServicioForm.Imagen3, visitaServicioForm.VisitaId, "Imagen3");
                    }

                    _visitaServicioFormData.Insert(visitaServicioForm);
                }
                
                var primeraVisita = visitasServicioForm.First();
                var Visita = _visitaServicioData.GetById(primeraVisita.VisitaId);

                //esta visita tiene asociados los visita Servicio Form
                //me traigo el primero, total todos los registros tienen las mismas imagnes
                var primerForm = Visita.Formularios.First();
                //ruta de imagenes
                string ruta1 = !string.IsNullOrEmpty(primerForm.Imagen1) ? primerForm.Imagen1 : null;
                string ruta2 = !string.IsNullOrEmpty(primerForm.Imagen2) ? primerForm.Imagen2 : null;
                string ruta3 = !string.IsNullOrEmpty(primerForm.Imagen3) ? primerForm.Imagen3 : null;

                var unidadNegocio = _unidadNegocioData.GetById(Visita.UnidadNegocioId);
                var formulario = _formularioData.GetById(primeraVisita.FormId);

                var UnidadNegocioNombre = unidadNegocio.Nombre;
                var ServicioNombre = "";
                //este mail hay que ponerlo abajo
                var supervisorNombre = Visita.Supervisor.Email;

                //Generar los PDFs solo una vez por cada formulario
                var pdfBytesList = GeneratePdfs(visitasServicioForm);
                //Preparar los detalles para los adjuntos de correo electrónico
                string toEmail = $"{supervisorNombre},miriam.betancourt@limpiolux.com.ar,fernando.soto@limpiolux.com.ar,pgomez@limpiolux.com.ar,abigioni@limpiolux.com.ar,marcela@ariesasociados.com.ar,operaciones@ariesasociados.com.ar,msanchez@limpiolux.com.ar";
                string subject = "Visita Servicio Preventores";
                string body = "Se ha registrado una visita. Se adjuntan los PDF de cada formulario cargado.";

                var attachments = new List<EmailAttachment>();

                if (Visita.UnidadNegocio.Id == 1) // limpiolux
                {
                    ServicioNombre = $"{Visita.ServicioPrestado.ClienteNombre} - {Visita.ServicioPrestado.CasaNro} - {Visita.ServicioPrestado.CasaNombre}";
                }
                else if (Visita.UnidadNegocio.Id >= 2 && Visita.UnidadNegocio.Id <= 5) // fbm - y las demás
                {
                    ServicioNombre = $"{Visita.ServicioPrestado.CasaNro} - {Visita.ServicioPrestado.CasaNombre}";
                }

                //Iterar solo una vez sobre los PDFs generados para crear los adjuntos
                foreach (var pdf in pdfBytesList)
                {
                    string fechaActual = DateTime.Now.ToString("dd-MM-yyyy");
                    string attachmentName = $"FormularioNro_{pdf.Key}_{Visita.Id}_{UnidadNegocioNombre}_{ServicioNombre}_{fechaActual}.pdf";

                    attachments.Add(new EmailAttachment
                    {
                        FileName = attachmentName,
                        Content = pdf.Value
                    });
                }

                //ver el tema de las imagenes para adjuntarlas al EMAIL
                //Validar y añadir las imágenes a los adjuntos solo si existen
                var imagePaths = new List<string> { ruta1, ruta2, ruta3 };
                
                foreach (var imagePath in imagePaths)
                {
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        string fullImagePath = HttpContext.Current.Server.MapPath(imagePath);

                        if (File.Exists(fullImagePath))
                        {
                            byte[] imageBytes = File.ReadAllBytes(fullImagePath);

                            attachments.Add(new EmailAttachment
                            {
                                FileName = Path.GetFileName(fullImagePath),
                                Content = imageBytes
                            });
                        }
                    }
                }

                _emailService.SendEmailWithAttachments(toEmail, subject, body, attachments);
                //no cambiar mensaje de error porque impacta en el cliente
                return Ok("OK");
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }


        private Dictionary<int, byte[]> GeneratePdfs(List<VisitaServicioForm> forms)
        {
            var pdfs = new Dictionary<int, byte[]>();

            var groupedForms = forms.GroupBy(f => f.FormId);

            foreach (var group in groupedForms)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    iTextSharp.text.Document doc = new iTextSharp.text.Document();
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    //Cargar la imagen
                    string imagePath = HttpContext.Current.Server.MapPath("~/Content/Images/limpiolux_logo.png");
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagePath);
                    logo.ScaleToFit(600f, 600f); //Escalar la imagen si es necesario
                    logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                    doc.Add(logo);

                    //Obtener la información de la visita
                    var firstForm = group.First(); 
                    var Visita = _visitaServicioData.GetById(firstForm.VisitaId);
                    var unidadNegocio = _unidadNegocioData.GetById(Visita.UnidadNegocioId);

                    var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    var paragraph = new Paragraph("Información de la Visita:", boldFont);
                    paragraph.SpacingAfter = 10f; 
                    doc.Add(paragraph);

                    doc.Add(new Paragraph($"Id Visita: {firstForm.VisitaId}"));
                    doc.Add(new Paragraph($"Unidad de Negocio: {Visita.UnidadNegocio.Nombre}"));

                    if (Visita.UnidadNegocio.Id == 1) // limpiolux
                    {
                        doc.Add(new Paragraph($"Servicio Prestado: {Visita.ServicioPrestado.ClienteNombre} - {Visita.ServicioPrestado.CasaNro} - {Visita.ServicioPrestado.CasaNombre}"));
                    }
                    else if (Visita.UnidadNegocio.Id >= 2 && Visita.UnidadNegocio.Id <= 5) // fbm - y las demas
                    {
                        doc.Add(new Paragraph($"Servicio Prestado: {Visita.ServicioPrestado.CasaNro} - {Visita.ServicioPrestado.CasaNombre}"));
                    }

                    doc.Add(new Paragraph($"Quien entrevista: {Visita.Cliente}"));

                    if (!string.IsNullOrEmpty(Visita.Cliente2))
                    {
                        doc.Add(new Paragraph($"Otro Servicio: {Visita.Cliente2}"));
                    }

                    doc.Add(new Paragraph($"Fecha de la Visita: {Visita.FechaVisita:dd-MM-yyyy}"));
                    doc.Add(new Paragraph($"Supervisor: {Visita.Supervisor.Nombre} - {Visita.Supervisor.Email}"));
                    doc.Add(new Paragraph($"Usuario: {Visita.Usuario.Name} - ({Visita.Usuario.Rol})"));
                    var versionParagraph = new Paragraph($"RG 4.4.6-02 - Version: {firstForm.version}");
                    versionParagraph.SpacingAfter = 10f; 
                    doc.Add(versionParagraph);


                    //agregar datos de form vehiculos, si es que estoy en ese form
                    if (!string.IsNullOrEmpty(Visita.ModeloVehiculo))
                    {
                        var datosVehiculoParagraph = new Paragraph("Datos de Vehículo:", boldFont);
                        doc.Add(datosVehiculoParagraph);
                        doc.Add(new Paragraph($"Modelo: {Visita.ModeloVehiculo}"));
                    }

                    if (!string.IsNullOrEmpty(Visita.Conductor))
                    {
                        doc.Add(new Paragraph($"Conductor: {Visita.Conductor}"));
                    }

                    if (Visita.TipoVehiculoId != null)
                    {
                        doc.Add(new Paragraph($"Tipo Vehículo: {Visita.TipoVehiculoId}"));
                    }

                    if (!string.IsNullOrEmpty(Visita.Dominio))
                    {
                        doc.Add(new Paragraph($"Dominio: {Visita.Dominio}"));
                    }


                    if (!string.IsNullOrEmpty(Visita.Proveedor))
                    {
                        var proveedorParagraph = new Paragraph($"Proveedor: {Visita.Proveedor}");
                        proveedorParagraph.SpacingAfter = 10f; 
                        doc.Add(proveedorParagraph);
                    }

                    //Agregar el nombre del formulario una sola vez
                    var formulario = _formularioData.GetById(firstForm.FormId);
                    var formularioNombre = new Paragraph($"{formulario.Nombre}", boldFont);
                    formularioNombre.SpacingAfter = 10f;
                    doc.Add(formularioNombre);

                    //Ahora iterar sobre los formularios en el grupo para los detalles
                    var itemCounter = 1;
                    var ComentarioGeneral = "";
                    foreach (var formGroup in group.GroupBy(f => f.Item))
                    {
                        var itemParagraph = new Paragraph($"{itemCounter}. {formGroup.Key}", boldFont);
                        itemParagraph.SpacingAfter = 5f; // Espaciado después del ítem
                        doc.Add(itemParagraph);
                        itemCounter++;

                        foreach (var form in formGroup)
                        {
                            //SubItem y Respuesta
                            var subItemParagraph = new Paragraph($"\u2022 {form.SubItem}: {form.Respuesta}");
                            subItemParagraph.SpacingAfter = 3f; // Espaciado después del subítem
                            doc.Add(subItemParagraph);

                            if (!string.IsNullOrEmpty(form.Comentario))
                            {
                                var redFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.RED);
                                var comentarioParagraph = new Paragraph($"Comentario: {form.Comentario}", redFont);
                                comentarioParagraph.SpacingAfter = 3f; // Espaciado después del comentario
                                doc.Add(comentarioParagraph);
                            }

                            ComentarioGeneral = form.ComentarioGeneral;
                        }
                    }

                    // Agregar el comentario general al final
                    if (!string.IsNullOrEmpty(ComentarioGeneral))
                    {
                        var redFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.RED);

                        //Añadir espaciado antes de agregar el comentario general
                        var comentarioGeneralParagraph = new Paragraph($"Comentario General: {ComentarioGeneral}", redFont);
                        comentarioGeneralParagraph.SpacingBefore = 10f; 
                        doc.Add(comentarioGeneralParagraph);
                    }

                    doc.Close();
                    pdfs.Add(group.Key, ms.ToArray());
                }
            }

            return pdfs;
        }

        private string GuardarImagen(string base64Image, int visitaId, string imageType)
        {
            try
            {
                string carpetaImagenes = HttpContext.Current.Server.MapPath("~/ImagenesVisitas/");

                if (!Directory.Exists(carpetaImagenes))
                {
                    Directory.CreateDirectory(carpetaImagenes);
                }

                var base64String = base64Image.Contains(",") ? base64Image.Substring(base64Image.IndexOf(",") + 1) : base64Image;
                var bytes = Convert.FromBase64String(base64String);

                //Generar el nombre de archivo único
                string fileName = $"{imageType}_{visitaId}_{Guid.NewGuid()}.jpg";
                string filePath = Path.Combine(carpetaImagenes, fileName);

                //Guardar la imagen en la carpeta del servidor
                File.WriteAllBytes(filePath, bytes);

                //Devolver la ruta de la imagen para guardarla en la base de datos
                return $"/ImagenesVisitas/{fileName}";
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al guardar la imagen: {ex.Message}"); // Cambiar a un sistema de logging si es necesario
                return null;
            }
        
        }
        [HttpGet]
        [Route("obtenerformulariosByIdVisita/{idVisita}")]
        public IHttpActionResult ObtenerFormulariosPorVisita(int idVisita)
        {
            try
            {   // Verificar si la visita existe
                var visitaExistente = _visitaServicioData.GetById(idVisita);
                if (visitaExistente == null)
                {
                    return BadRequest($"La visita con ID {idVisita} no existe.");
                }


                // Obtener formularios relacionados con la visita especificada
                var formularios = _visitaServicioFormData.List()
                                    .Where(f => f.VisitaId == idVisita)
                                    .ToList();

                return Ok(formularios);
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

    }
}
