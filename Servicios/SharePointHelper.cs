using cash_server.Data;
using cash_server.Models;
using cash_server.Servicios;
using Microsoft.SharePoint.Client;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Web;
using File = System.IO.File;

public class SharePointHelper
{
    private ClientContext context;
    private Web web;
   
    public SharePointHelper(string siteUrl, string username, string password)
    {
        try
        {
            context = new ClientContext(siteUrl);
            SecureString securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }

            context.Credentials = new SharePointOnlineCredentials(username, securePassword);
            web = context.Web;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al conectar con SharePoint: " + ex.Message);
        }
    }


    public void InsertVisitaServicioForm(VisitaServicio visitaServicio, VisitaServicioForm visitaServicioForm)
    {
       FormularioData _formularioData = new FormularioData();

        //Obtener la lista de SharePoint 
        List listaFormularios;
        try
        {
            //listaFormularios = web.Lists.GetByTitle("Oportunidad de Mejora");
            listaFormularios = web.Lists.GetByTitle("Oportunidades de Mejora V2"); 
            VerificarColumnas(listaFormularios);
        }
        catch (Exception ex)
        {
            throw new Exception("No se pudo obtener la lista 'Oportunidades de Mejora': " + ex.Message);
        }

        //Crear un nuevo item para la lista
        ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
        ListItem listItem = listaFormularios.AddItem(itemCreateInfo);
        var formulario = _formularioData.GetById(visitaServicioForm.FormId);

        listItem["Formulario"] = formulario.Nombre;
        listItem["UnidadNegocio"] = visitaServicio.UnidadNegocio.Nombre;

        if (visitaServicio.UnidadNegocio.Nombre == "FBM S.A.")
        {
            listItem["Cliente"] = "sin datos";
        }
        else
        {
            listItem["Cliente"] = visitaServicio.ServicioPrestado.ClienteNombre;
        }

        listItem["ServicioPrestado"] = visitaServicio.ServicioPrestado.CasaNro + visitaServicio.ServicioPrestado.CasaNombre;

        if (!string.IsNullOrEmpty(visitaServicio.Cliente2))
        {
            listItem["OtroServicio"] = visitaServicio.Cliente2;
        }
        else
        {
            listItem["OtroServicio"] = DBNull.Value;  // o un valor predeterminado si es necesario
        }

        listItem["Preventor"] = visitaServicio.Usuario.Name;

        string descripcion = visitaServicioForm.Item + " - " + visitaServicioForm.SubItem + " - Rpta: " + visitaServicioForm.Respuesta;

        if (!string.IsNullOrEmpty(visitaServicioForm.Comentario))
        {
            descripcion += " - Comentario:  " + visitaServicioForm.Comentario; // Añades un salto de línea antes de los comentarios
        }

        if (!string.IsNullOrEmpty(visitaServicioForm.ComentarioGeneral))
        {
            descripcion += " - Comentario gral:  " +  visitaServicioForm.ComentarioGeneral; // Añades un salto de línea antes del comentario general
        }

        //listItem["Descripci_x00f3_n"] = descripcion;
        listItem["Descripcion"] = descripcion;
        listItem["Fecha"] = visitaServicio.FechaVisita;
        listItem["Supervisor"] = visitaServicio.Supervisor.Nombre;

        if (!string.IsNullOrEmpty(visitaServicioForm.Imagen1))
        {

            string imagen1Url = UploadImageToSharePoint(visitaServicioForm.Imagen1, "ImagenesVisitas", "Imagen1.jpg", "/ImagenesVisitas");

            string baseUrl = "https://limpiolux.sharepoint.com"; 
            string imagen1UrlCompleta = baseUrl + imagen1Url;    

            listItem["Imagen1"] = imagen1UrlCompleta;
        }
        

        if (!string.IsNullOrEmpty(visitaServicioForm.Imagen2))
        {
            string imagen2Url = UploadImageToSharePoint(visitaServicioForm.Imagen2, "ImagenesVisitas", "Imagen2.jpg", "/ImagenesVisitas");

            string baseUrl = "https://limpiolux.sharepoint.com";  
            string imagen2UrlCompleta = baseUrl + imagen2Url;   

            listItem["Imagen2"] = imagen2UrlCompleta;
        }

        if (!string.IsNullOrEmpty(visitaServicioForm.Imagen3))
        {
            string imagen3Url = UploadImageToSharePoint(visitaServicioForm.Imagen3, "ImagenesVisitas", "Imagen3.jpg", "/ImagenesVisitas");

            string baseUrl = "https://limpiolux.sharepoint.com";  
            string imagen3UrlCompleta = baseUrl + imagen3Url;    

            listItem["Imagen3"] = imagen3UrlCompleta;
        }

        try
        {
            listItem.Update();
            context.ExecuteQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al guardar el item en SharePoint: " + ex.Message);
        }
    }
    

    public string UploadImageToSharePoint(string imagePath, string libraryName, string fileName, string folderPath)
    {
        //Establecer la URL del sitio de SharePoint y el contexto
        
        // url prueba https://limpiolux.sharepoint.com/sites/TestCASH
        string siteUrl = "https://limpiolux.sharepoint.com/sites/Preventores";
        ClientContext context = new ClientContext(siteUrl);

        //Crear un SecureString para la contraseña
        SecureString password = new SecureString();
        foreach (char c in "Sard1na.3400")  // Usa la contraseña correcta aquí
        {
            password.AppendChar(c);
        }

        //Autenticación utilizando SharePointOnlineCredentials
        context.Credentials = new SharePointOnlineCredentials("pautomate@limpiolux.com.ar", password);
        Web web = context.Web;

        //Cargar explícitamente la propiedad ServerRelativeUrl de web
        context.Load(web, w => w.ServerRelativeUrl);
        context.ExecuteQuery();

        //Obtener la propiedad ServerRelativeUrl de web
        string serverRelativeUrl = web.ServerRelativeUrl;

        // Verificar si la biblioteca de documentos existe
        List library = null;
        try
        {
            library = web.Lists.GetByTitle(libraryName);
            context.Load(library);
            context.ExecuteQuery();
        }
        catch (Exception)
        {
            // La biblioteca no existe, así que la creamos
            ListCreationInformation creationInfo = new ListCreationInformation
            {
                Title = libraryName,
                TemplateType = (int)ListTemplateType.DocumentLibrary // Tipo de plantilla para una biblioteca de documentos
            };

            library = web.Lists.Add(creationInfo);
            context.ExecuteQuery();
        }

        context.Load(library.RootFolder); // Cargar la carpeta raíz completa de la biblioteca
        context.ExecuteQuery();

        string rootFolderUrl = library.RootFolder.ServerRelativeUrl;

        string finalFolderPath = rootFolderUrl; 

        if (!string.IsNullOrEmpty(folderPath))
        {
            finalFolderPath = Path.Combine(finalFolderPath, folderPath); 
        }

        if (!string.IsNullOrEmpty(imagePath))
        {
            string fullImagePath = HttpContext.Current.Server.MapPath(imagePath);  

            if (File.Exists(fullImagePath))
            {
                byte[] fileBytes = File.ReadAllBytes(fullImagePath);

                string uniqueFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                //Cargar el archivo en SharePoint
                MemoryStream stream = new MemoryStream(fileBytes);

                //Ahora construimos la URL correctamente, que comience con '/'
                string uploadedFileUrl = Path.Combine(finalFolderPath, uniqueFileName);  

                uploadedFileUrl = uploadedFileUrl.Replace("\\", "/"); 

                //la ruta debe comenzar con '/sites/TestCASH' 
                if (!uploadedFileUrl.StartsWith(serverRelativeUrl))
                {
                    uploadedFileUrl = serverRelativeUrl + uploadedFileUrl; //ruta completa
                }

                FileCreationInformation newFile = new FileCreationInformation
                {
                    ContentStream = stream,
                    Url = uploadedFileUrl  //Ruta en la biblioteca de documentos
                };

                //Subir el archivo a SharePoint
                Microsoft.SharePoint.Client.File uploadFile = library.RootFolder.Files.Add(newFile);
                context.Load(uploadFile);
                context.ExecuteQuery(); 

                string finalUrl = uploadedFileUrl; 

                finalUrl = finalUrl.Replace("\\", "/"); 

                Console.WriteLine("Imagen cargada con éxito. URL: " + finalUrl);
                return finalUrl;  // Ahora retorna la URL correcta
            }
            else
            {
                throw new FileNotFoundException("No se pudo encontrar el archivo en la ruta especificada: " + fullImagePath);
            }
        }

        return string.Empty;  //Si la ruta es vacía o el archivo no existe
    }


    //verificar con esto que columnas tuene
    public void VerificarColumnas(List listaFormularios)
    {
        try
        {
            var fields = listaFormularios.Fields;
            context.Load(fields);
            context.ExecuteQuery();

            foreach (var field in fields)
            {
                Console.WriteLine("Columna: " + field.Title + " - Nombre Interno: " + field.InternalName);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las columnas de la lista: " + ex.Message);
        }
    }
}
