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
        private readonly VisitaServicioFormData _visitaServicioFormData;
        private readonly UsuarioData _usuarioData;

        public VisitaServicioController()
        {
            _visitaServicioData = new VisitaServicioData();
            _visitaServicioFormData = new VisitaServicioFormData();
            _usuarioData = new UsuarioData();
        }

        //se le envia un json con los datos en el body
        /*
         {
            "ServicioPrestadoId": "1",
            "Cliente": "Empresa ABC",
            "UnidadNegocioId": "1",
            "FechaVisita": "2024-03-13T10:00:00",
            "ModeloVehiculo": "Toyota Corolla",
            "Conductor": "Juan Pérez",
            "TipoVehiculoId": 1,
            "Dominio": "ABC123",
            "Proveedor": "Taller Mecánico XYZ",
            "SupervisorId": 264,
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

        //obtiene todas las visitas de todos los usuarios (con sus respectivos form y respuestas)
        //si no hay ninguna visita retorna un array vacio
        [HttpGet]
        [Route("visitasgetall")]
        public IHttpActionResult ObtenerTodasLasVisitas()
        {
            try
            {
                var visitas = _visitaServicioData.List();

                foreach (var visita in visitas)
                {
                    var formularios = _visitaServicioFormData.List().Where(f => f.VisitaId == visita.Id).ToList();
                    
                    visita.Formularios = formularios;
                }

                if (visitas.Any())
                {
                    return Ok(visitas);
                }
                else
                {
                    return Content(HttpStatusCode.NoContent, "No hay visitas cargadas!");
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }
        //este endpoint se podria usar que dado un UsuarioId preventor, traer los datos de sus visitas
        [HttpGet]
        [Route("visitasgetallbyId/{usuarioId}")]
        public IHttpActionResult ObtenerTodasLasVisitas(int usuarioId)
        {
            try
            {
                var usuarios = _usuarioData.List();
                var usuario = usuarios.FirstOrDefault(u => u.Id == usuarioId && u.Activo);

                if (usuario == null)
                {
                    return Content(HttpStatusCode.NotFound, new { message = "El Usuario con el Id proporcionado no existe o no está activo" });
                }
                var visitas = _visitaServicioData.List().Where(v => v.UsuarioId == usuarioId);

                foreach (var visita in visitas)
                {
                    var formularios = _visitaServicioFormData.List().Where(f => f.VisitaId == visita.Id).ToList();

                    visita.Formularios = formularios;
                }

                if (visitas.Any())
                {
                    return Ok(visitas);
                }
                else
                {
                    return Content(HttpStatusCode.NoContent, "No hay visitas cargadas para el Usuario dado.");
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

    }

}


