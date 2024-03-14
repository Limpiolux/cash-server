using cash_server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace cash_server.Models
{
    public class Formulario : IEntity
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Nombre { get; set;}
    }
}