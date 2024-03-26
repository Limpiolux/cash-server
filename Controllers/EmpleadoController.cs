using cash_server.Data;
using cash_server.Models;
using cash_server.Servicios;
using cash_server.SharedKernel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Razor.Text;

namespace cash_server.Controllers
{
    [RoutePrefix("empleado")]
    [EnableCors(origins: "http://localhost:5173", headers: "*", methods: "*")]
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
        [HttpGet]
        [Route("getallsupervisores")]
        public async Task<IHttpActionResult> GetSupervisores()
        {
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
                            supervisor.Rol = RolEmpleado.Supervisor;
                            supervisor.Activo = true;
                            supervisor.Usuario = null;
                            
                            //puse esto porque si no viene con email, tira error en la insercion, pincha el programa, ya que mail es obligatorio
                            //entonces con el chatch capturo la excepcion para que no se pare el programa
                            try
                            {
                                //if (!string.IsNullOrWhiteSpace(supervisor.Email))
                                //{
                                    _empleadoData.Insert(supervisor);
                              
                                //}

                            }
                            catch (DbEntityValidationException ex)
                            {
                                // Manejar la excepción de validación y para que no pinche el programa
                                foreach (var validationErrors in ex.EntityValidationErrors)
                                {
                                    foreach (var validationError in validationErrors.ValidationErrors)
                                    {
                                        // Registrar los detalles del error de validación
                                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                                    }
                                }
                            }

                          
                        }
                    }

                    //una vez cargados en la tabla traigo los supervisores para retornarlos
                    var nuevosSupervisores = _empleadoData.List().Where(e => e.Rol == RolEmpleado.Supervisor);

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

    }
}