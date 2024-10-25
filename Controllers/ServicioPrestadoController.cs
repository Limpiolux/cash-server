using Antlr.Runtime;
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

namespace cash_server.Controllers
{
    [RoutePrefix("clientecasa")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ServicioPrestadoController : ApiController
    {
        private readonly ServicioPrestadoData _servicioPrestadoData;
        private readonly UnidadNegocioData _unidadNegocioData;

        public ServicioPrestadoController()
        {
            _servicioPrestadoData = new ServicioPrestadoData();
            _unidadNegocioData = new UnidadNegocioData();
        }

        //casas limpiolux
        [Route("getallservicioscasasLimpiolux/{unidadNegocioId}")]
        public async Task<IHttpActionResult> GetServiciosCasas(int unidadNegocioId)
        {
            try
            {
                var httpService = new HttpService<IEnumerable<ServicioPrestado>>("https://localhost:44362");
                var serviciosCasas = await httpService.GetAsync("/clientecasa/getallclientescasasLimpiolux");

                if (serviciosCasas != null && serviciosCasas.Any())
                {
                    //obtengo datos de unidad de negocio LImpio
                    var unidadNegocio = _unidadNegocioData.List()
                        .FirstOrDefault(u => u.Id == unidadNegocioId && unidadNegocioId == 1 && u.Nombre == "LIMPIOLUX S.A." && u.Cuit == "30540984626" && u.Activo);

                    if (unidadNegocio == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { message = "No se encontró la unidad de negocio Limpiolux" });
                    }

                    //PROCESO PARA INSERCION EN LA TABLA 
                    foreach (var servicioCasa in serviciosCasas)
                    {
                        //veriufica si ya existe
                        var existe = _servicioPrestadoData.GetByCasaNroyNombre(servicioCasa);

                        if (existe == null)
                        {
                            ServicioPrestado serPres = new ServicioPrestado();
                            serPres.ClienteNro = servicioCasa.ClienteNro;
                            serPres.ClienteNombre = servicioCasa.ClienteNombre;
                            serPres.CasaNro = servicioCasa.CasaNro;
                            serPres.CasaNombre = servicioCasa.CasaNombre;
                            serPres.UnidadNegocioId = unidadNegocio.Id;
                            serPres.Activo = true;

                            try
                            {
                                _servicioPrestadoData.Insert(serPres);
                            }
                            catch (DbEntityValidationException ex)
                            {
                                // Manejar la excepción de validación
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
                        else
                        {
                            ServicioPrestado serPresUpdate = new ServicioPrestado();
                            serPresUpdate.ClienteNro = servicioCasa.ClienteNro;
                            serPresUpdate.ClienteNombre = servicioCasa.ClienteNombre;
                            serPresUpdate.CasaNro = servicioCasa.CasaNro;
                            serPresUpdate.CasaNombre = servicioCasa.CasaNombre;
                            serPresUpdate.UnidadNegocioId = unidadNegocio.Id;
                            serPresUpdate.Activo = existe.Activo;
                            serPresUpdate.Id = existe.Id; //este es el de mi base de datos

                            try
                            {
                                _servicioPrestadoData.Update(serPresUpdate);
                            }
                            catch (DbEntityValidationException ex)
                            {
                                // Manejar la excepción de validación
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
                    //aca debo discriminar y traer solo los clientes de Limpiolux
                    var nuevosServiciosCasas = _servicioPrestadoData.List().Where(sp => sp.UnidadNegocioId == unidadNegocio.Id && sp.UnidadNegocioId == 1).ToList();

                    return Json(nuevosServiciosCasas); //Retornar la lista para el cliente
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron Casas (servicios prestados)" });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("getallclientescasaFBM/{unidadNegocioId}")]
        public async Task<IHttpActionResult> GetClientesCasaFBM(int unidadNegocioId)
        {
            try
            {
                var httpService = new HttpService<IEnumerable<ServicioPrestado>>("https://localhost:44362");
                var clientesCasaFBM = await httpService.GetAsync("/clientecasaFBM/sharepointclientescasaFBM");

                if (clientesCasaFBM != null && clientesCasaFBM.Any())
                {
                    //Obtener datos de la unidad de negocio FBM
                    var unidadNegocio = _unidadNegocioData.List()
                        .FirstOrDefault(u => u.Id == unidadNegocioId && unidadNegocioId == 2 && u.Nombre == "FBM S.A." && u.Activo);

                    if (unidadNegocio == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { message = "No se encontró la unidad de negocio FBM" });
                    }

                    //Proceso para inserción en la tabla
                    foreach (var clienteCasaFBM in clientesCasaFBM)
                    {

                        var existe = _servicioPrestadoData.GetByCasaNroyNombre(clienteCasaFBM);

                        if (existe == null)
                        {
                            ServicioPrestado serPres = new ServicioPrestado();
                            //serPres.ClienteNro = clienteCasaFBM.casa; //estos no hace falta poner, en estos casos en FBM no existen en las listas
                            //estos datos, asi que quedan comentados y se cargan con null.
                            //serPres.ClienteNombre = clienteCasaFBM.Nombre; 
                            serPres.CasaNro = clienteCasaFBM.CasaNro;
                            serPres.CasaNombre = clienteCasaFBM.CasaNombre;
                            serPres.UnidadNegocioId = unidadNegocio.Id;
                            serPres.Localidad = string.IsNullOrEmpty(clienteCasaFBM.Localidad) ? null : clienteCasaFBM.Localidad;
                            serPres.Activo = true;

                            try
                            {
                                _servicioPrestadoData.Insert(serPres);
                            }
                            catch (DbEntityValidationException ex)
                            {
                                // Manejar la excepción de validación
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
                        else
                        {
                            ServicioPrestado serPresUpdate = new ServicioPrestado();
                            serPresUpdate.CasaNro = clienteCasaFBM.CasaNro;
                            serPresUpdate.CasaNombre = clienteCasaFBM.CasaNombre;
                            serPresUpdate.UnidadNegocioId = unidadNegocio.Id;
                            serPresUpdate.Localidad = string.IsNullOrEmpty(clienteCasaFBM.Localidad) ? null : clienteCasaFBM.Localidad;
                            serPresUpdate.Activo = existe.Activo;
                            serPresUpdate.Id = existe.Id;

                            try
                            {
                                _servicioPrestadoData.Update(serPresUpdate);
                            }
                            catch (DbEntityValidationException ex)
                            {
                                // Manejar la excepción de validación
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
                    //aca debo discriminar y traer solo los clientes de FBM ser
                    var nuevosServiciosCasas = _servicioPrestadoData.List().Where(sp => sp.UnidadNegocioId == unidadNegocio.Id && sp.UnidadNegocioId == 2).ToList();
                    return Json(nuevosServiciosCasas); // Retornar la lista para el cliente
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron clientes de casa FBM" });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }
        }

        //este trae las propias casas de t&t y DistMaster de esta misma bd y las retorna 
        // 3 Tyt, 4 DistMaster
        [HttpGet]
        [Route("getallclientescasaTyTYDistMaster/{unidadNegocioId}")]
        public IHttpActionResult GetClientesCasaTyT(int unidadNegocioId)
        {
            try
            {
                var casas = _servicioPrestadoData.List()
                        .Where(c => c.UnidadNegocioId == unidadNegocioId && c.Activo).ToList();

                if (casas.Any())
                {
                    return Json(casas);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron casas para la unidad de negocio seleccionada" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

        //este trae las propias casa generica de Otro servicio... para completar luego el campo otro servicio
        // 5 es unid de negocio Otro Servicio
        [HttpGet]
        [Route("getallclientescasaOtroServicio/{unidadNegocioId}")]
        public IHttpActionResult GetClientesOtroServicios(int unidadNegocioId)
        {
            try
            {
                var casas = _servicioPrestadoData.List()
                        .Where(c => c.UnidadNegocioId == unidadNegocioId && unidadNegocioId == 5 && c.Activo).ToList();

                if (casas.Any())
                {
                    return Json(casas);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron casas para la unidad de negocio seleccionada" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

        //casas de ceiling id = 6 ceiling
        [HttpGet]
        [Route("getallservicioscasasCeiling/{unidadNegocioId}")]
        public async Task<IHttpActionResult> GetServiciosCasasCeiling(int unidadNegocioId)
        {
            try
            {
                var httpService = new HttpService<IEnumerable<ServicioPrestado>>("https://localhost:44303");
                var serviciosCasas = await httpService.GetAsync("/clientecasa/getallclientescasasCeiling");

                if (serviciosCasas != null && serviciosCasas.Any())
                {
                    //obtengo datos de unidad de negocio LImpio
                    var unidadNegocio = _unidadNegocioData.List()
                        .FirstOrDefault(u => u.Id == unidadNegocioId && unidadNegocioId == 6 && u.Nombre == "Ceiling" && u.Activo);

                    if (unidadNegocio == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { message = "No se encontró la unidad de negocio Limpiolux" });
                    }

                    //PROCESO PARA INSERCION EN LA TABLA 
                    foreach (var servicioCasa in serviciosCasas)
                    {
                        //veriufica si ya existe
                        var existe = _servicioPrestadoData.GetByCasaNroyNombre(servicioCasa);

                        if (existe == null)
                        {
                            ServicioPrestado serPres = new ServicioPrestado();
                            serPres.ClienteNro = servicioCasa.ClienteNro;
                            serPres.ClienteNombre = servicioCasa.ClienteNombre;
                            serPres.CasaNro = servicioCasa.CasaNro;
                            serPres.CasaNombre = servicioCasa.CasaNombre;
                            serPres.UnidadNegocioId = unidadNegocio.Id;
                            serPres.Activo = true;

                            try
                            {
                                _servicioPrestadoData.Insert(serPres);
                            }
                            catch (DbEntityValidationException ex)
                            {
                                // Manejar la excepción de validación
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
                        else
                        {
                            ServicioPrestado serPresUpdate = new ServicioPrestado();
                            serPresUpdate.ClienteNro = servicioCasa.ClienteNro;
                            serPresUpdate.ClienteNombre = servicioCasa.ClienteNombre;
                            serPresUpdate.CasaNro = servicioCasa.CasaNro;
                            serPresUpdate.CasaNombre = servicioCasa.CasaNombre;
                            serPresUpdate.UnidadNegocioId = unidadNegocio.Id;
                            serPresUpdate.Activo = existe.Activo;
                            serPresUpdate.Id = existe.Id; //este es el de mi base de datos

                            try
                            {
                                _servicioPrestadoData.Update(serPresUpdate);
                            }
                            catch (DbEntityValidationException ex)
                            {
                                // Manejar la excepción de validación
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
                    //aca debo discriminar y traer solo los clientes de Limpiolux
                    var nuevosServiciosCasas = _servicioPrestadoData.List().Where(sp => sp.UnidadNegocioId == unidadNegocio.Id && sp.UnidadNegocioId == 6).ToList();

                    return Json(nuevosServiciosCasas); //Retornar la lista para el cliente
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron Casas (servicios prestados)" });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }
        }

    }
}