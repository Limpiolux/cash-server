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


namespace cash_server.Controllers
{
    [RoutePrefix("user")]
    [EnableCors(origins: "http://localhost:5173", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private readonly ApiDbContext _dbContext;
        private readonly EncryptionService _encryptionService;

        public UserController()
        {
            _dbContext = new ApiDbContext();
            _encryptionService = new EncryptionService();
        }

        [HttpPost]
        [Route("register")]
        public IHttpActionResult RegistrarUsuario([FromBody] Usuario user)
        {
            try
            {
                //campos obligatoirios
                if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Mail) || string.IsNullOrEmpty(user.Password))
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "Todos los campos (Nombre, Correo electrónico, Contraseña) son obligatorios." });
                }
                //el rol debe ser valido es decir Preventor o Adminstrador
                if (!Enum.IsDefined(typeof(RolUsuario), user.Rol))
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "Rol de usuario inválido. Los roles válidos son 'Preventor' (1) y 'Administrador' (2)." });
                }

                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Mail == user.Mail);
                if (existingUser != null)
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "El correo electrónico ya está registrado. Por favor, utiliza otro correo electrónico." });
                }

                var hashedPassword = _encryptionService.EncryptPassword(user.Password);

                // Crear un nuevo usuario
                var nuevoUsuario = new Usuario
                {
                    Name = user.Name,
                    Mail = user.Mail,
                    Password = hashedPassword,
                    Rol = user.Rol //el rol tiene que venir en el objeto user
                };

                _dbContext.Users.Add(nuevoUsuario); 
                _dbContext.SaveChanges(); 

                return Json(new { message = "Registro exitoso" });
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y devolver un mensaje de error genérico junto con el código de error HTTP 500
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor: "+ Convert.ToString(ex.Message) });
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
