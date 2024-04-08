using cash_server.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public class UnidadNegocio : IEntity
    {
        [Key]
        [Required]
        [Column(Order = 1)]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Cuit { get; set; }
        //Activo = 1 = true, esta activo, habilitado... Activo = 0 usuario dado de baja
        //por default le ponemos que esta deshabilitado
        [Required]
        public bool Activo { get; set; } = false;

    }
}