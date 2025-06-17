using App.Metrics.Formatters.Prometheus;
using BenchmarkDotNet.Portability;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using cash_server.Data;
using cash_server.Models;
using cash_server.Servicios;
using cash_server.SharedKernel;
using Prometheus;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Services.Client;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Razor.Text;

namespace cash_server.Controllers
{
    [RoutePrefix("empleado")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    /// <summary>
    /// Controlador para operaciones relacionadas con supervisores.
    /// </summary>
    public class EmpleadoController : ApiController
    {
        private readonly EmpleadoData _empleadoData;
        private readonly UnidadNegocioData _unidadNegocioData;
        public EmpleadoController()
        {
            _empleadoData = new EmpleadoData();
            _unidadNegocioData = new UnidadNegocioData();
        }


        [HttpGet]
        [Route("getallpreventores")]
        [SwaggerOperation("GetAllValues")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(IEnumerable<string>))]
        public IHttpActionResult GetAllPreventores()
        {
            try
            {
                var empleados = _empleadoData.List();
                var preventores = empleados.Where(e => e.Rol == RolEmpleado.Preventor && e.Activo == true).ToList();

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

        //trae los supervisores de otro endpoint,le carga el campo supervisor y activo= true
        //debe verificar antes que este supervisor no este en la tabla... si no esta lo inserta
        //una vez insertados los supervisores en la tabla, los devuelve para ser leidos posteriomente por el cliente
        //LIMPIOLUX
        [HttpGet]
        [Route("getallsupervisores")]
        public async Task<IHttpActionResult> GetSupervisores()
        {
            var unidadNegocio = _unidadNegocioData.List().FirstOrDefault(u => u.Nombre.ToLower().Contains("limpiolux"));
            
            try
            {
                var httpService = new HttpService<IEnumerable<Empleado>>("https://localhost:44362");
                var supervisores = await httpService.GetAsync("/supervisor/getallsupervisores");

                if (supervisores != null && supervisores.Any())
                {

                    //PROCESO PARA INSERCION EN LA TABLA EMPLEADOS (SUPERVISORES)
                    foreach (var supervisor in supervisores)
                    {
                        // Verificar si el supervisor ya existe en la base de datos con ese Email, si esta Activo, y si Rol=2
                        var existente = _empleadoData.GetByEmailAndActivoSupervisor(supervisor.Email);

                        if (existente == null)
                        {
                            Empleado superv = new Empleado();
                            superv.Rol = RolEmpleado.Supervisor;
                            superv.Activo = true;
                            superv.Nombre = supervisor.Nombre;
                            superv.Email = supervisor.Email;
                            superv.Usuario = null;
                            superv.UnidadNegocio_id = unidadNegocio.Id;
                            //puse esto porque si no viene con email, tira error en la insercion, pincha el programa, ya que mail es obligatorio
                            //entonces con el chatch capturo la excepcion para que no se pare el programa
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(supervisor.Email))
                                {
                                    _empleadoData.Insert(superv);

                                }

                            }
                            catch (DbEntityValidationException ex)
                            {
                                foreach (var validationError in ex.EntityValidationErrors)
                                {
                                    Console.WriteLine($"Entidad de tipo {validationError.Entry.Entity.GetType().Name} tiene los siguientes errores de validación:");
                                    foreach (var error in validationError.ValidationErrors)
                                    {
                                        Console.WriteLine($"- Propiedad: {error.PropertyName}, Error: {error.ErrorMessage}");
                                    }
                                }
                            }


                        }
                        else
                        {
                            //puse esto porque si no viene con email, tira error en la insercion, pincha el programa, ya que mail es obligatorio
                            //entonces con el chatch capturo la excepcion para que no se pare el programa
                            try
                            {
                                    //superUdate.Usuario no le cargo nada lo dejo como esta
                                    Empleado superUpdate = new Empleado();
                                    superUpdate.Rol = RolEmpleado.Supervisor;
                                    superUpdate.Activo = true;
                                    superUpdate.Nombre = supervisor.Nombre;
                                    superUpdate.Email = supervisor.Email;
                                    superUpdate.Id = existente.Id;
                                    //superUpdate.Usuario_id = existente.Usuario_id; //usuario no tienen los supervisores
                                    superUpdate.UnidadNegocio_id = existente.UnidadNegocio_id;


                                if (!string.IsNullOrWhiteSpace(supervisor.Email))
                                {
                                    _empleadoData.Update(superUpdate);

                                }

                            }
                            catch (DbEntityValidationException ex)
                            {
                                foreach (var validationError in ex.EntityValidationErrors)
                                {
                                    Console.WriteLine($"Entidad de tipo {validationError.Entry.Entity.GetType().Name} tiene los siguientes errores de validación:");
                                    foreach (var error in validationError.ValidationErrors)
                                    {
                                        Console.WriteLine($"- Propiedad: {error.PropertyName}, Error: {error.ErrorMessage}");
                                    }
                                }
                            }
                        }
                    }

                    //una vez cargados en la tabla traigo los supervisores para retornarlos
                    var nuevosSupervisores = _empleadoData.List().Where(e => e.Rol == RolEmpleado.Supervisor && e.Activo == true);

                    return Json(nuevosSupervisores); //retorna la lista para el cliente
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron supervisores" });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }

        }
        //trae los supervisores de otro endpoint,le carga el campo supervisor y activo= true
        //debe verificar antes que este supervisor no este en la tabla... si no esta lo inserta
        //una vez insertados los supervisores en la tabla, los devuelve para ser leidos posteriomente por el cliente
        //CEILING
        //no se va a usar en principio porque son los mimos quelimpiolux
        [HttpGet]
        [Route("getallsupervisoresCeiling")]
        public async Task<IHttpActionResult> GetSupervisoresCeiling()
        {
            
            
            
            
            
            var unidadNegocio = _unidadNegocioData.List().FirstOrDefault(u => u.Nombre.ToLower().Contains("ceiling"));
            try
            {
                var httpService = new HttpService<IEnumerable<Empleado>>("https://localhost:44303"); //ceiling
                var supervisores = await httpService.GetAsync("/supervisor/getallsupervisores");

                if (supervisores != null && supervisores.Any())
                {

                    //PROCESO PARA INSERCION EN LA TABLA EMPLEADOS (SUPERVISORES)
                    foreach (var supervisor in supervisores)
                    {
                        // Verificar si el supervisor ya existe en la base de datos con ese Email, si esta Activo, y si Rol=2
                        var existente = _empleadoData.GetByEmailAndActivoSupervisorCeiling(supervisor.Email);

                        if (existente == null)
                        {
                            Empleado superv = new Empleado();
                            superv.Rol = RolEmpleado.Supervisor;
                            superv.Activo = true;
                            superv.Nombre = supervisor.Nombre;
                            superv.Email = supervisor.Email;
                            superv.Usuario = null;
                            superv.UnidadNegocio_id = unidadNegocio.Id;

                            //puse esto porque si no viene con email, tira error en la insercion, pincha el programa, ya que mail es obligatorio
                            //entonces con el chatch capturo la excepcion para que no se pare el programa
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(supervisor.Email))
                                {
                                    _empleadoData.Insert(superv);

                                }

                            }
                            catch (DbEntityValidationException ex)
                            {
                                foreach (var validationError in ex.EntityValidationErrors)
                                {
                                    Console.WriteLine($"Entidad de tipo {validationError.Entry.Entity.GetType().Name} tiene los siguientes errores de validación:");
                                    foreach (var error in validationError.ValidationErrors)
                                    {
                                        Console.WriteLine($"- Propiedad: {error.PropertyName}, Error: {error.ErrorMessage}");
                                    }
                                }
                            }


                        }
                        else
                        {
                            //puse esto porque si no viene con email, tira error en la insercion, pincha el programa, ya que mail es obligatorio
                            //entonces con el chatch capturo la excepcion para que no se pare el programa
                            try
                            {
                                //superUdate.Usuario no le cargo nada lo dejo como esta
                                Empleado superUpdate = new Empleado();
                                superUpdate.Rol = RolEmpleado.Supervisor;
                                superUpdate.Activo = true;
                                superUpdate.Nombre = supervisor.Nombre;
                                superUpdate.Email = supervisor.Email;
                                superUpdate.Id = existente.Id;
                                //uperUpdate.Usuario_id = existente.Usuario_id;  //esto es medio al cuete porque no teinen usuario
                                superUpdate.UnidadNegocio_id = existente.UnidadNegocio_id;

                                if (!string.IsNullOrWhiteSpace(supervisor.Email))
                                {
                                    _empleadoData.Update(superUpdate);

                                }

                            }
                            catch (DbEntityValidationException ex)
                            {
                                foreach (var validationError in ex.EntityValidationErrors)
                                {
                                    Console.WriteLine($"Entidad de tipo {validationError.Entry.Entity.GetType().Name} tiene los siguientes errores de validación:");
                                    foreach (var error in validationError.ValidationErrors)
                                    {
                                        Console.WriteLine($"- Propiedad: {error.PropertyName}, Error: {error.ErrorMessage}");
                                    }
                                }
                            }
                        }
                    }

                    //una vez cargados en la tabla traigo los supervisores para retornarlos
                    var nuevosSupervisores = _empleadoData.List().Where(e => e.Rol == RolEmpleado.Supervisor && e.Activo == true);

                    return Json(nuevosSupervisores); //retorna la lista para el cliente
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron supervisores" });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }

        }
        //esto lo hice para probar
        /// <summary>
        /// Obtiene el correo electrónico de un supervisor por su ID.
        /// </summary>
        /// <param name="idSupervisor">ID del supervisor.</param>
        /// <returns>Correo electrónico del supervisor.</returns>
        /// /// <remarks>
        /// Este método obtiene el correo electrónico de un supervisor utilizando su ID.
        /// </remarks>
        [HttpGet]
        [Route("getsupervisoremail/{idSupervisor}")]
        //[SwaggerOperation(Descriptor = "Obtiene el correo electrónico de un supervisor por su ID.")]
        [SwaggerResponse(HttpStatusCode.OK, "Correo electrónico del supervisor obtenido correctamente.", typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No se encontró ningún supervisor con el ID proporcionado.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error interno del servidor.")]
        public IHttpActionResult GetSupervisorEmail(int idSupervisor)
        {
            try
            {
                var supervisor = _empleadoData.GetById(idSupervisor);

                if (supervisor != null && supervisor.Rol == RolEmpleado.Supervisor)
                {
                    return Json(new { email = supervisor.Email });
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontró ningún supervisor con el ID proporcionado." });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("getallsupervisoresFBM")]
        public IHttpActionResult GetSupervisoresFBM()
        {
            try
            {
                var supervisoresFBM = _empleadoData.List().Where(e => e.Rol == RolEmpleado.Supervisor && e.Activo && e.UnidadNegocio_id == 2).ToList();

                if (supervisoresFBM.Any())
                {
                    return Json(supervisoresFBM);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron supervisores" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

        [HttpGet]
        [Route("getallsupervisoresTYT")]
        public IHttpActionResult GetSupervisoresTYT()
        {
            try
            {
                var supervisoresTYT = _empleadoData.List().Where(e => e.Rol == RolEmpleado.Supervisor && e.Activo && e.UnidadNegocio_id == 3).ToList();

                if (supervisoresTYT.Any())
                {
                    return Json(supervisoresTYT);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron supervisores" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

        [HttpGet]
        [Route("getallsupervisoresDistMaster")]
        public IHttpActionResult GetSupervisoresDistMaster()
        {
            try
            {
                var supervisoresDM = _empleadoData.List().Where(e => e.Rol == RolEmpleado.Supervisor && e.Activo && e.UnidadNegocio_id == 4).ToList();

                if (supervisoresDM.Any())
                {
                    return Json(supervisoresDM);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron supervisores" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

        [HttpGet]
        [Route("getallsupervisoresOtroServicio")]
        public IHttpActionResult GetSupervisoresOtroServicio()
        {
            try
            {
                var supervisoresOS = _empleadoData.List().Where(e => e.Rol == RolEmpleado.Supervisor && e.Activo && e.UnidadNegocio_id == 5).ToList();

                if (supervisoresOS.Any())
                {
                    return Json(supervisoresOS);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron supervisores" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

    }
}