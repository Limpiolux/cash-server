using cash_server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace cash_server.Controllers
{
    [RoutePrefix("formulario")]
    public class FormularioController: ApiController
    {
        private readonly FormularioData _formularioData;
        public FormularioController()
        {
            _formularioData = new FormularioData();
        }

        //Inyecta FormularioData en el constructor
        /*public FormularioController(FormularioData formularioData)
        {
            _formularioData = formularioData;
        }*/

        [HttpGet]
        [Route("getallformularios")]
        public IHttpActionResult GetAllFormularios()
        {
            try
            {
                var formularios = _formularioData.List();

                if (formularios.Any())
                {
                    return Json(formularios);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron formularios" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }
    }
}