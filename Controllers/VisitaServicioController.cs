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
    [RoutePrefix("visitaservicio")]
    [EnableCors(origins: "http://localhost:5173", headers: "*", methods: "*")]
    public class VisitaServicioController: ApiController
    {
        private readonly VisitaServicioData _visitaServicioData;

        public VisitaServicioController()
        {
            _visitaServicioData = new VisitaServicioData();
        }
        //se le envia un json con los datos en el body
        /*
         {
            "ServicioPrestado": "Reparación de equipo",
            "Cliente": "Empresa ABC",
            "UnidadNegocio": "Sucursal 123",
            "FechaVisita": "2024-03-13T10:00:00",
            "ModeloVehiculo": "Toyota Corolla",
            "Conductor": "Juan Pérez",
            "TipoVehiculoId": 1,
            "Dominio": "ABC123",
            "Proveedor": "Taller Mecánico XYZ",
            "SupervisorId": 264,
            "PreventorId": 1,
            "UsuarioId": 3
        }
          ModeloVehiculo,Conductor,TipoVehiculoId, Dominio, Proveedor pueden ir en null si
          si no se está en el formulario de vehiculos.
        
         */
        [HttpPost]
        [Route("crear")]
        public IHttpActionResult CrearVisitaServicio([FromBody] VisitaServicio visitaServicio)
        {
           
            try
            {

                if (visitaServicio == null)
                {
                    return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
                }
                _visitaServicioData.Insert(visitaServicio);
                return Ok(new { id = visitaServicio.Id });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + Convert.ToString(ex.Message) });
            }
        }
    }

}
