using cash_server.Data;
using cash_server.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using cash_server.Servicios;
using System.Security.Claims;
using System.Text;
using System.Web.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using IdentityModel.OidcClient;
using System.Web;
using System.Web.Http.Cors;
using System.Collections.Generic;
using cash_server.SharedKernel;
using Antlr.Runtime.Misc;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Web.Helpers;


namespace cash_server.Controllers
{
    [RoutePrefix("user")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private readonly ApiDbContext _dbContext;
        private readonly EncryptionService _encryptionService;
        private readonly EmpleadoData _empleadoData;
        private readonly UsuarioData _usuarioData;

        public UserController()
        {
            _dbContext = new ApiDbContext();
            _encryptionService = new EncryptionService();
            _empleadoData = new EmpleadoData();
            _usuarioData = new UsuarioData();

        }

        [HttpPost]
        [Route("register")]
        public IHttpActionResult RegistrarUsuario([FromBody] Usuario user)
        {
            /*el usuario tiene, si es preventor:
            Name: se saca del select del preventor, tomando el nombre (el select preventor se rellena con un endpoint con los datos del preventor)
            Mail: se saca del select del preventor, tomando el mail. En el select de preventor se debe concatenar email - nombre
            Password: se saca del campo de texto 
            Rol se saca del selec de Rol, a Rol se le pasa el texto Preventor, proveniente del select

            {
                "Name": "Hernán Ingrassia",
                "Mail": "heringrassia@gmail.com",
                "Password": "1234",
                "Rol": "Preventor"
            }

            si es Administrador, el usuario va a tener:
            Name: se saca del cuadro de texto donde se escribe Nombre y apellido
            Email: se saca del texto donde se escribe el Email
            Password: Se saca del campo de texto donde se escribe el pass
            Rol: se saca del select, se le pasaría el texto del select en este caso Administrador
            */
            try
            {
                // Verificar si el rol es válido
                /*if (user.Rol != RolUsuario.Preventor && user.Rol != RolUsuario.Administrador)
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "Rol de usuario inválido. Los roles válidos son 'Preventor' y 'Administrador'." });
                }*/

                //el rol debe ser valido es decir Preventor o Adminstrador
                if (!Enum.IsDefined(typeof(RolUsuario), user.Rol))
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "Rol de usuario inválido. Los roles válidos son 'Preventor' (1) y 'Administrador' (2)." });
                }

                //aca se chequea que si ya hay un usuario con ese mail y que esté activo, no te deberia dejar ingresar el nuevo user
                //si el mail ya esta pero esta Inactivo,es decir Activo= false, ahi si podrias ingresar el usuario
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Mail == user.Mail && u.Activo);

                if (existingUser != null)
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "El Email ya está registrado y el usuario asociado está activo. Por favor, utilice otro Email." });
                }

                // Validar campos obligatorios comunes a ambos roles
                if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Mail) || string.IsNullOrEmpty(user.Password))
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "Todos los campos (Nombre, Correo electrónico, Contraseña) son obligatorios." });
                }

                var nuevoUsuario = new Usuario
                {
                    Name = user.Name,
                    Mail = user.Mail,
                    Password = _encryptionService.EncryptPassword(user.Password),
                    Rol = user.Rol
                };

                // Si el rol es Preventor, cargar datos de Empleado
                if (user.Rol == RolUsuario.Preventor)
                {
                    //busca en la tabla ese email y que el rol sea preventor
                    var preventor = _dbContext.Empleados.FirstOrDefault(e => e.Email == user.Mail && e.Rol == RolEmpleado.Preventor);
                    if (preventor == null)
                    {
                        return Content(HttpStatusCode.BadRequest, new { error = "No se encontró un preventor con el correo electrónico proporcionado." });
                    }
                    //se asgnan los datos del preventor al usuario
                    nuevoUsuario.Mail = preventor.Email;
                    nuevoUsuario.Name = preventor.Nombre;
                }

                //Si el rol es Administrador guardo el usuario y no tengo que hacer mas nada
                _dbContext.Users.Add(nuevoUsuario);
                _dbContext.SaveChanges(); //VER ACA SI ABAJO CUANDO NO SE PUEDE ACTUALIZAR EL ID_ISUARIO EN LA TABLA EMPLEADOS, VER SI SE PUEDE HACER UN ROLLBACK PARA QUE NO INSERTE NINGUN USUARIO

                //guardar en la tabla Empleados y relaciona el Prevendor con el usuario (IdUsuario)
                if (user.Rol == RolUsuario.Preventor)
                {

                    var preventor = _dbContext.Empleados.FirstOrDefault(e => e.Email == user.Mail && e.Rol == RolEmpleado.Preventor);
                    if (preventor != null)
                    {
                        preventor.Usuario_id = nuevoUsuario.Id;
                        _empleadoData.Update(preventor);
                    }
                }

                return Json(new { message = "Registro exitoso" });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + Convert.ToString(ex.Message) });
            }
        }


        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] Models.LoginRequest request)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Mail == request.Mail);
                if (user == null)
                {
                    return Content(HttpStatusCode.NotFound, new { error = "Usuario no encontrado" });
                }

                var passwordValid = _encryptionService.VerifyPassword(request.Password, user.Password);
                if (!passwordValid)
                {
                    return Content(HttpStatusCode.Unauthorized, new { error = "Credenciales incorrectas" });
                }

                // Crear token JWT
                var jwtSecret = WebConfigurationManager.AppSettings["JwtSecret"];
                if (string.IsNullOrEmpty(jwtSecret))
                {
                    return Content(HttpStatusCode.InternalServerError, new { error = "La clave secreta JWT no está configurada correctamente" });
                }

                var key = Encoding.ASCII.GetBytes(jwtSecret);
                var tokenHandler = new JwtSecurityTokenHandler();
                
                //para poder enlazar el token del usuario con el nombre
                var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Mail),
                    new Claim("Mail", user.Mail),
                });

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = DateTime.UtcNow.AddHours(1), // Token válido por 1 hora
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Json(new { message = "Inicio de sesión exitoso", token = tokenString });
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y devolver un mensaje de error genérico junto con el código de error HTTP 500
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

        [HttpPost]
        [Route("validatetoken")]
        public IHttpActionResult ValidateToken([FromBody] TokenRequest request)
        {
            try
            {
                string jwtToken = request.Token;

                if (string.IsNullOrEmpty(jwtToken))
                {
                    return Content(HttpStatusCode.Unauthorized, new { error = "Se requiere un token de autenticación" });
                }

                //se obtiene la clave secreta JWT de la configuración
                var jwtSecret = WebConfigurationManager.AppSettings["JwtSecret"];
                if (string.IsNullOrEmpty(jwtSecret))
                {
                    return Content(HttpStatusCode.InternalServerError, new { error = "La clave secreta JWT no está configurada correctamente" });
                }

                var key = Encoding.ASCII.GetBytes(jwtSecret);

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtSecurityToken = (JwtSecurityToken)validatedToken;
                var decodedToken = new
                {
                    Issuer = jwtSecurityToken.Issuer,
                    Subject = jwtSecurityToken.Subject,
                    Expires = jwtSecurityToken.ValidTo
                };

                return Json(new { message = "Token válido", decoded = decodedToken });
            }
            catch (SecurityTokenException)
            {
                return Content(HttpStatusCode.Unauthorized, new { error = "Token inválido" });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        
        }

        [HttpGet]
        [Route("getuserdata")]
        public IHttpActionResult GetUserData()
        {
            try
            {
                // Obtener el token del encabezado Authorization
                var authHeader = HttpContext.Current.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(authHeader))
                {
                    return Content(HttpStatusCode.Unauthorized, new { error = "Se requiere un token de autenticación en el encabezado Authorization" });
                }

                // Validar si el encabezado Authorization tiene el formato correcto (Bearer token)
                var tokenParts = authHeader.Split(' ');
                if (tokenParts.Length != 2 || !tokenParts[0].Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                {
                    return Content(HttpStatusCode.Unauthorized, new { error = "Formato de token incorrecto. Debe tener el formato 'Bearer token'" });
                }

                // Obtener el token JWT
                var token = tokenParts[1];

                var jwtSecret = WebConfigurationManager.AppSettings["JwtSecret"];
                if (string.IsNullOrEmpty(jwtSecret))
                {
                    return Content(HttpStatusCode.InternalServerError, new { error = "La clave secreta JWT no está configurada correctamente" });
                }

                var key = Encoding.ASCII.GetBytes(jwtSecret);

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtSecurityToken = (JwtSecurityToken)validatedToken;

                var userEmail = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "Mail")?.Value;

                // Buscar usuario por Mail relacionado con ese token
                var usuario = _dbContext.Users.FirstOrDefault(u => u.Mail == userEmail);

                if (usuario == null)
                {
                    return Content(HttpStatusCode.NotFound, new { error = "No se encontró ningún usuario asociado con el token proporcionado" });
                }

                return Json(usuario);
            }
            catch (SecurityTokenException)
            {
                return Content(HttpStatusCode.Unauthorized, new { error = "Token inválido" });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }


        [HttpGet]
        [Route("listroles")]
        public IHttpActionResult ListarRolesUsuario()
        {
            var rolesUsuario = Enum.GetValues(typeof(RolUsuario))
                                   .Cast<RolUsuario>()
                                   .Select(r => new { Id = (int)r, Rol = r.ToString() })
                                   .ToList();
            return Json(rolesUsuario);
        }

        //trae todos los usuarios activos
        [HttpGet]
        [Route("activeusers")]
        public IHttpActionResult GetActiveUsers()
        {
            try
            {
                var activeUsers = _usuarioData.List().Where(u => u.Activo).ToList();

                if (activeUsers.Any())
                {
                    return Json(activeUsers);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron usuarios activos." });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }
        }
        //put porque voy a modificar un recurso
        [HttpPut]
        [Route("delete/{userId}")]
        public IHttpActionResult DeactivateUser(int userId)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    return Content(HttpStatusCode.NotFound, new { error = "No se encontró ningún usuario con el ID proporcionado." });
                }

                // Verificar si el usuario tiene visitas asociadas
                var hasVisits = _dbContext.VisitaServicios.Any(v => v.UsuarioId == userId);

                if (hasVisits)
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "No se puede eliminar el usuario porque tiene visitas asociadas." });
                }


                //desactivo el usuario
                user.Activo = false;
                _dbContext.SaveChanges();

                //Si el usuario es un preventor, buscar ese Empleado (preventor) y desasociar el usuario tambien, porque ese usuario no va a estar mas activo
                if (user.Rol == RolUsuario.Preventor)
                {
                    var empleado = _dbContext.Empleados.FirstOrDefault(e => e.Usuario_id == userId);
                    
                    if (empleado != null)
                    {
                        empleado.Usuario_id = null;
                        _dbContext.SaveChanges();
                    }
                }

              
                return Json(new { message = $"El usuario con ID {userId} ha sido dado de baja exitosamente." });
            }
            catch (Exception ex)
            {
               
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }
        }

        //siempre se le pasan todos los datos completos, con las mopdificaciones o los mismos datos que no se cambian
        //password se le pasa, pero nunca se va a modificar
        [HttpPut]
        [Route("edit/{userId}")]
        public IHttpActionResult EditUser(int userId, [FromBody] Usuario user)
        {
            /*DATOS A ENVIAR
                {
                   "Name": "Hernan Ingrassia",
                   "Mail": "heringrassia@gmail.com",
                   "Password": "1234",
                   "Rol": "1" / "2"
                }

            */
            /*
                si el usuario no desea cambiar la contraseña, el campo viene vacío
                no hago nada no actualizo la contraseña
                si el campo viene != de vacio, la encripto y actualizo el campo
             
             */
            try
            {
                // Validar que se haya proporcionado un objeto Usuario en el cuerpo de la solicitud
                if (user == null)
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "Se requiere un objeto Usuario en el cuerpo de la solicitud." });
                }

                // Validar campos obligatorios del objeto Usuario
                //el password no necesariamente es obligatorio que llegue desde el form
                if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Mail) || user.Rol == 0 || !Enum.IsDefined(typeof(RolUsuario), user.Rol))
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "Los campos Nombre, Email y Rol son obligatorios." });
                }

                //verificar si el usuario existe y está activo
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId && u.Activo);
                if (existingUser == null)
                {
                    return Content(HttpStatusCode.NotFound, new { error = "No se encontró ningún usuario activo con el ID proporcionado" });
                }

                //1) Verificar si se intenta cambiar el rol a preventor
                if (existingUser.Rol == RolUsuario.Administrador && user.Rol == RolUsuario.Preventor)
                {
                    //antes de insertar un empleado, revisa en la tabla si hay algun registro con similares caracteristicas
                    // y si haylo desactiva.
                    var empleadosSimilares = _dbContext.Empleados.Where(e => e.Email.Contains(user.Mail) &&
                                                          e.Nombre.Contains(user.Name) &&
                                                          e.Activo &&
                                                          e.Rol == RolEmpleado.Preventor).ToList();

                    // Iterar sobre los empleados encontrados y desactivarlos
                    foreach (var empleado in empleadosSimilares)
                    {
                        empleado.Activo = false;
                        _empleadoData.Update(empleado);
                    }

                    //Insertar en la tabla Empleados
                    var nuevoEmpleado = new Empleado
                    {
                        Nombre = user.Name,
                        Email = user.Mail,
                        Rol = RolEmpleado.Preventor,
                        Activo = true,
                        Usuario_id = existingUser.Id //asocio al usuario admin, al cual le estoy cambiando el rol a preventor
                    };
                    _empleadoData.Insert(nuevoEmpleado);


                    // Actualizar datos en la tabla Usuarios
                    existingUser.Name = user.Name;
                    existingUser.Mail = user.Mail;
                    existingUser.Rol = user.Rol; //preventor
                    if (!string.IsNullOrWhiteSpace(user.Password))
                    {
                        existingUser.Password = _encryptionService.EncryptPassword(user.Password);
                    }

                    _usuarioData.Update(existingUser);

                    return Json(new { message = "Usuario actualizado correctamente." });
                }
                //2) Verificar si se intenta cambiar el rol a Administrador 
                else if (existingUser.Rol == RolUsuario.Preventor && user.Rol == RolUsuario.Administrador)
                {
                    //Desasociar el usuario de la tabla Empleados
                    var empleado = _dbContext.Empleados.FirstOrDefault(e => e.Usuario_id == userId && e.Activo);
                    if (empleado != null)
                    {
                        empleado.Usuario_id = null;
                        _dbContext.SaveChanges();
                    }

                    //Actualizar datos en la tabla Usuarios
                    existingUser.Name = user.Name;
                    existingUser.Mail = user.Mail;
                    existingUser.Rol = user.Rol; //administrador
                    if (!string.IsNullOrWhiteSpace(user.Password))
                    {
                        existingUser.Password = _encryptionService.EncryptPassword(user.Password);
                    }
                    _usuarioData.Update(existingUser);

                    return Json(new { message = "Usuario actualizado correctamente." });
                }
                //3) el rol de admin se mantiene solo actualizo usuarios
                else if (existingUser.Rol == RolUsuario.Administrador && user.Rol == RolUsuario.Administrador)
                {
                    //Actualizar datos en la tabla Usuarios
                    existingUser.Name = user.Name;
                    existingUser.Mail = user.Mail;
                    existingUser.Rol = user.Rol; //administrador
                    if (!string.IsNullOrWhiteSpace(user.Password))
                    {
                        existingUser.Password = _encryptionService.EncryptPassword(user.Password);
                    }
                    _usuarioData.Update(existingUser);

                    return Json(new { message = "Usuario actualizado correctamente." });
                }
                //4)preventor de usuarios se mantiene como preventor.
                else if (existingUser.Rol == RolUsuario.Preventor && user.Rol == RolUsuario.Preventor)
                {
                    //Actualizar datos en la tabla Usuarios
                    existingUser.Name = user.Name;
                    existingUser.Mail = user.Mail;
                    existingUser.Rol = user.Rol; //preventor
                    if (!string.IsNullOrWhiteSpace(user.Password))
                    {
                        existingUser.Password = _encryptionService.EncryptPassword(user.Password);
                    }
                    _usuarioData.Update(existingUser);

                    //Actualizo datos del del user preventor en la tabla empleados para que machee los datos del user con los del preventor
                    var empleado = _dbContext.Empleados.FirstOrDefault(e => e.Usuario_id == userId && e.Activo);
                    
                    empleado.Nombre = user.Name;
                    empleado.Email = user.Mail;
                    empleado.Rol = RolEmpleado.Preventor; //sigue siendo preventor porque no se le cambio
                    empleado.Activo = true;
                    empleado.Usuario_id = userId; //mismo idUser
                    _empleadoData.Update(empleado);

                    return Json(new { message = "Usuario y preventor actualizados correctamente." });
                }
                
                //en cualquier otro caso que no cubra los anteriores
                else
                {
                    
                    return Content(HttpStatusCode.BadRequest, new { error = "Error! o No tienes permiso para modificar los datos de este usuario." });
                    


                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }
        }

        //dado un userId devuelve el Rol de ese user
        [HttpGet]
        [Route("getuserrole/{userId}")]
        public IHttpActionResult GetUserRole(int userId)
        {
            try
            {
                var user = _usuarioData.List().FirstOrDefault(u => u.Id == userId && u.Activo);

                if (user == null)
                {
                    return Content(HttpStatusCode.NotFound, new { error = "No se encontró ningún usuario activo con el ID proporcionado." });
                }

                var userRole = user.Rol.ToString();
                return Json(new { Rol = userRole });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: " + ex.Message });
            }
        }


        public class TokenRequest
        {
            public string Token { get; set; }
        }

        //en caso que se quiera liberar memoria
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
