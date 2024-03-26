using cash_server.Data;
using cash_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace cash_server.Controllers
{
    [RoutePrefix("visitaservicioform")]
    [EnableCors(origins: "http://localhost:5173", headers: "*", methods: "*")]
    public class VisitaServicioFormController:ApiController
    {
        private readonly VisitaServicioFormData _visitaServicioFormData;

        public VisitaServicioFormController()
        {
            _visitaServicioFormData = new VisitaServicioFormData();
        }
        /*datos a enviar
        [
        {
            "VisitaId": 7,
            "FormId": 3,
            "Item": "Ejemplo de item 1",
            "SubItem": "Ejemplo de subitem 1",
            "Comentario": "Ejemplo de comentario 1",
            "Respuesta": "Ejemplo de respuesta 1"
        },
        {
            "VisitaId": 7,
            "FormId": 2,
            "Item": "Ejemplo de item 2",
            "SubItem": "Ejemplo de subitem 2",
            "Comentario": null,
            "Respuesta": "Ejemplo de respuesta 2"
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
                    _visitaServicioFormData.Insert(visitaServicioForm);
                }

                return Ok("Registros insertados correctamente.");
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }
    }
}
