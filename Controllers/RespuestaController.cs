using cash_server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace cash_server.Controllers
{
    [RoutePrefix("respuesta")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RespuestaController: ApiController
    {

        private readonly RespuestaData _respuestaData;

        public RespuestaController()
        {
            _respuestaData = new RespuestaData();
        }

        [HttpGet]
        [Route("getrespuestasbyitemid/{itemId}")]
        public IHttpActionResult GetRespuestasByItemId(int itemId)
        {
            try
            {
                var respuestas = _respuestaData.List().Where(r => r.ItemId == itemId).ToList();
                if (respuestas.Any())
                {
                    return Json(respuestas);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron respuestas para el Item especificado" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }
    }
}