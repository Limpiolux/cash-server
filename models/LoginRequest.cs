using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public class LoginRequest
    {   //para manejar la respuesta del request luego de enviar los datos para el login
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}