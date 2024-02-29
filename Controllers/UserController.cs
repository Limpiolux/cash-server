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



namespace cash_server.Controllers
{
    [RoutePrefix("user")]
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
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Mail == user.Mail);
                if (existingUser != null)
                {
                    return Content(HttpStatusCode.BadRequest, new { error = "El correo electrónico ya está registrado. Por favor, utiliza otro correo electrónico." });
                }

                var hashedPassword = _encryptionService.EncryptPassword(user.Password);
                user.Password = hashedPassword;

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return Json(new { message = "Registro exitoso" });
            }
            catch (Exception ex)
            {
                //Manejar cualquier excepción y devolver un mensaje de error genérico junto con el código de error HTTP 500
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
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
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Mail)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1), //Token válido por 1 hora
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
        public IHttpActionResult GetUserData(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { error = "Se requiere un token de autenticación" });
                }

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

                //usuario relacionado a ese token
                var usuarios = _dbContext.Users.ToList();

                return Json(usuarios);
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
