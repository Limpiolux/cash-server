using cash_server.Data;
using cash_server.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace cash_server.Controllers
{
    [RoutePrefix("empleado")]
    public class EmpleadoController : ApiController
    {
        private readonly EmpleadoData _empleadoData;

        public EmpleadoController()
        {
            _empleadoData = new EmpleadoData();
        }

        [HttpGet]
        [Route("getallpreventores")]
        public IHttpActionResult GetAllPreventores()
        {
            try
            {
                var empleados = _empleadoData.List();
                var preventores = empleados.Where(e => e.Rol == RolEmpleado.Preventor).ToList();

                if (preventores.Any())
                {
                    return Json(preventores);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron preventores" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

    }
}