﻿using cash_server.Data;
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


namespace cash_server.Controllers
{
    [RoutePrefix("user")]
    [EnableCors(origins: "http://localhost:5173", headers: "*", methods: "*")]
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
        public IHttpActionResult ListarRoles()
        {
            var roles = Enum.GetNames(typeof(RolUsuario));
            return Json(roles);
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
