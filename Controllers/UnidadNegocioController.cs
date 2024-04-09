using cash_server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace cash_server.Controllers
{
    [RoutePrefix("unidadnegocio")]
    public class UnidadNegocioController : ApiController
    {
        private readonly UnidadNegocioData _unidadNegocioData;

        public UnidadNegocioController()
        {
            _unidadNegocioData = new UnidadNegocioData();
        }

        [HttpGet]
        [Route("getallunidadesnegocio")]
        public IHttpActionResult GetAllUnidadesNegocio()
        {
            try
            {
                var unidadesNegocio = _unidadNegocioData.List();

                if (unidadesNegocio.Any())
                {
                    return Json(unidadesNegocio);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron unidades de negocio" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

    }
}