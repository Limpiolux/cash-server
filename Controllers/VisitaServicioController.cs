using cash_server.Data;
using cash_server.Models;
using cash_server.SharedKernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace cash_server.Controllers
{
    [RoutePrefix("visitaservicio")]
    [EnableCors(origins: "https://localhost:5173", headers: "*", methods: "*")]
    public class VisitaServicioController : ApiController
    {
        private readonly VisitaServicioData _visitaServicioData;
        private readonly VisitaServicioFormData _visitaServicioFormData;
        private readonly UsuarioData _usuarioData;
        private readonly EmpleadoData _empleadoData;
        private readonly UnidadNegocioData _unidadNegocioData;
        private readonly ServicioPrestadoData _servicioPrestadoData;

        public VisitaServicioController()
        {
            _visitaServicioData = new VisitaServicioData();
            _visitaServicioFormData = new VisitaServicioFormData();
            _usuarioData = new UsuarioData();
            _empleadoData = new EmpleadoData();
            _unidadNegocioData = new UnidadNegocioData();
            _servicioPrestadoData = new ServicioPrestadoData();
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

        //obtiene todas las visitas de TODOS los usuarios (con sus respectivos form y respuestas)
        //si no hay ninguna visita retorna un array vacio
        [HttpGet]
        [Route("visitasgetall")]
        public IHttpActionResult ObtenerTodasLasVisitas()
        {
            try
            {
                var visitas = _visitaServicioData.List();

                if (visitas.Any())
                {
                    foreach (var visita in visitas)
                    {
                        var formularios = _visitaServicioFormData.List().Where(f => f.VisitaId == visita.Id).ToList();
                        visita.Formularios = formularios;
                    }
                    return Ok(visitas);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron visitas" });
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
                    return Content(HttpStatusCode.BadRequest, "No hay visitas cargadas para el Usuario dado.");
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }
        /********************************/
        //ESTE EN REALIAD NO SE VA A USAR
        //endpoint para filtrar visitas por supervisor
        [HttpGet]
        [Route("visitasgetallbysupervisor/{supervisorId}")]
        public IHttpActionResult ObtenerVisitasPorSupervisor(int supervisorId)
        {
            try
            {
                var visitas = _visitaServicioData.List();

                if (!visitas.Any())
                {
                    return Content(HttpStatusCode.BadRequest, new { message = "No hay visitas cargadas." });
                }

                var supervisores = _empleadoData.List();
                
                var supervisor = supervisores.FirstOrDefault(s => s.Id == supervisorId && s.Activo && s.Rol == RolEmpleado.Supervisor);

                if (supervisor == null)
                {
                    return Content(HttpStatusCode.NotFound, new { message = "El Supervisor con el Id proporcionado no existe o no está activo" });
                }

                var visitasPorSupervisor = visitas.Where(v => v.SupervisorId == supervisorId).ToList();

                if (visitasPorSupervisor.Any())
                {
                    return Ok(visitasPorSupervisor);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No hay visitas para el Supervisor proporcionado" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }
    
        //EN REALIDAD NO SE SI ESTE SE VA A UTILIZAR
        //endpoint que devuelve las visitas de los preventores
        [HttpGet]
        [Route("visitasgetallbypreventores")]
        public IHttpActionResult ObtenerVisitasPorPreventores()
        {
            try
            {
                //traes los preventores
                var preventores = _empleadoData.List().Where(u => u.Rol == RolEmpleado.Preventor && u.Activo);

                if (!preventores.Any())
                {
                    return Content(HttpStatusCode.BadRequest, new { message = "No hay preventores cargados." });
                }

                var visitas = new List<VisitaServicio>();

                //recorro y busco la visitas que hayan sido gestionadas por el usuario preventor --> idusuario = idpreventor
                foreach (var preventor in preventores)
                {
                    var visitasPreventor = _visitaServicioData.List().Where(v => v.UsuarioId == preventor.Id);
                    
                    visitas.AddRange(visitasPreventor);
                }
                
                if (visitas.Any())
                {
                    foreach (var visita in visitas)
                    {
                        var formularios = _visitaServicioFormData.List().Where(f => f.VisitaId == visita.Id).ToList();
                        visita.Formularios = formularios;
                    }
                    return Ok(visitas);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, new { message = "No hay visitas cargadas de los preventores." });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }
        //EN REALIDAD ESTE NO SE VA A USAR
        [HttpGet]
        [Route("visitasgetallbyrol/{rol}")]
        public IHttpActionResult ObtenerVisitasPorRol(RolUsuario rol) //rol Preventor o Administrador
        {
            try
            {
                var usuarios = _usuarioData.List().Where(u => u.Rol == rol && u.Activo);

                if (!usuarios.Any())
                {
                    return Content(HttpStatusCode.BadRequest, new { message = "No hay usuarios con el rol especificado cargados." });
                }

                var visitas = new List<VisitaServicio>();

                //recorro los usuarios preventores o administradores, y busco las visitas que haya cargado ese usuario
                foreach (var usuario in usuarios)
                {
                    var visitasUsuario = _visitaServicioData.List().Where(v => v.UsuarioId == usuario.Id);
                    visitas.AddRange(visitasUsuario);
                }

                if (visitas.Any())
                {
                    foreach (var visita in visitas)
                    {
                        var formularios = _visitaServicioFormData.List().Where(f => f.VisitaId == visita.Id).ToList();
                        visita.Formularios = formularios;
                    }
                    return Ok(visitas);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, new { message = "No hay visitas cargadas para el rol especificado." });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

        /********************/

        //endpoint para filtrar por todos los parametros, del listado de visitas
        [HttpGet]
        [Route("filtrar")]
        public IHttpActionResult FiltrarVisitas(
            int? unidadNegocioId = null,
            int? servicioPrestadoId = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int? supervisorId = null,
            int? usuarioId = null,
            RolUsuario? rolUsuario = null)
        {
            try
            {
                if (unidadNegocioId != null)
                {
                    var unidadNegocio = _unidadNegocioData.GetById(unidadNegocioId.Value);
                    
                    if (unidadNegocio == null)
                    {
                        return Content(HttpStatusCode.BadRequest, new { message = "La unidad de negocio especificada no existe." });
                    }
                }

                if (servicioPrestadoId != null)
                {
                    var servicioPrestado = _servicioPrestadoData.GetById(servicioPrestadoId.Value);
                    
                    if (servicioPrestado == null)
                    {
                        return Content(HttpStatusCode.BadRequest, new { message = "El servicio prestado no existe." });
                    }
                }
                //obtengo las visitas
                var visitas = _visitaServicioData.List();

                if (!visitas.Any())
                {
                    return Content(HttpStatusCode.BadRequest, new { message = "No hay visitas cargadas." });
                }

                //valido que el supervisorId sea valido
                var supervisores = _empleadoData.List();

                var supervisor = supervisores.FirstOrDefault(s => s.Id == supervisorId || s.Activo && s.Rol == RolEmpleado.Supervisor);

                if (supervisor == null)
                {
                    return Content(HttpStatusCode.NotFound, new { message = "El Supervisor con el Id proporcionado no existe o no está activo" });
                }

                //valido que el usuarioId sea valido
                var usuarios = _usuarioData.List();

                var usuario = usuarios.FirstOrDefault(u => u.Id == usuarioId && u.Activo);

                if (usuario == null)
                {
                    return Content(HttpStatusCode.NotFound, new { message = "El Usuario con el Id proporcionado no existe o no está activo" });
                }
                //validoque el rol de usuario sea valido
                if (rolUsuario != null && (rolUsuario != RolUsuario.Preventor && rolUsuario != RolUsuario.Administrador))
                {
                    return Content(HttpStatusCode.BadRequest, new { message = "El Rol de Usuario especificado no es válido." });
                }
                //filtro por unidadde negocio
                if (unidadNegocioId != null)
                {
                    visitas = visitas.Where(v => v.UnidadNegocioId == unidadNegocioId);
                }
                //filtro por servicio prestado
                if (servicioPrestadoId != null)
                {
                    visitas = visitas.Where(v => v.ServicioPrestadoId == servicioPrestadoId);
                }
                //filtro por fechas
                if (fechaDesde != null && fechaHasta != null)
                {
                    visitas = visitas.Where(v => v.FechaVisita >= fechaDesde && v.FechaVisita <= fechaHasta).ToList();
                }
                else if (fechaDesde != null)
                {
                    visitas = visitas.Where(v => v.FechaVisita >= fechaDesde).ToList();
                }
                else if (fechaHasta != null)
                {
                    visitas = visitas.Where(v => v.FechaVisita <= fechaHasta).ToList();
                }
                //filtro por supervisor
                if (supervisorId != null)
                {
                    visitas = visitas.Where(v => v.SupervisorId == supervisorId);
                }
                //filtro por usuario
                if (usuarioId != null)
                {
                    visitas = visitas.Where(v => v.UsuarioId == usuarioId);
                }

                if (rolUsuario != null)
                {
                    visitas = visitas.Where(v => v.Usuario.Rol == rolUsuario);
                }

                //si hay alguna visita cargo los formularios de la visita
                if (visitas.Any())
                {
                    foreach (var visita in visitas)
                    {
                        var formularios = _visitaServicioFormData.List().Where(f => f.VisitaId == visita.Id).ToList();
                        visita.Formularios = formularios;
                    }
                    return Ok(visitas);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron visitas que coincidan con los filtros proporcionados." });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

        [HttpGet]
        [Route("obtenerVisitaPorId/{id}")]
        public IHttpActionResult ObtenerVisitaPorId(int id)
        {
            try
            {
                var visita = _visitaServicioData.GetById(id);

                if (visita == null)
                {
                    return Content(HttpStatusCode.NotFound, new { message = "La visita con el Id proporcionado no se encontró." });
                }
                
                var formularios = _visitaServicioFormData.List().Where(f => f.VisitaId == visita.Id).ToList();
                visita.Formularios = formularios;

                return Ok(visita);
            }
            catch (Exception ex)
            {
                // Si hay un error interno del servidor, devuelve un error 500 Internal Server Error
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }
        }



    }

}


