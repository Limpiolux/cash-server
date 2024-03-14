using cash_server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public class Usuario: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}