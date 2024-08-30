using cash_server.Data;
using cash_server.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace cash_server.Controllers
{
    [RoutePrefix("formulario")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        [HttpGet]
        [Route("listatipovehiculos")]
        public IHttpActionResult ListarTiposVehiculos()
        {
            var tipos_vehiculos = Enum.GetValues(typeof(TipoVehiculo))
                                      .Cast<TipoVehiculo>()
                                      .Select(v => new { Id = (int)v, Tipo = v.ToString() })
                                      .ToList();
            return Json(tipos_vehiculos);
        }

        [HttpGet]
        [Route("getformulario/{id}")]
        public IHttpActionResult GetFormularioById(int id)
        {
            try
            {
                var formulario = _formularioData.GetById(id);

                if (formulario != null)
                {
                    return Json(formulario);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "Formulario no encontrado" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }
    }
}