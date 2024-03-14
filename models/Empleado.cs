using cash_server.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using cash_server.Interfaces;

namespace cash_server.Models
{
    public class Empleado : IEntity
    {
        //les puse requeridas porque los supervisores se van a cargar de un endpoint
        [Key]
        [Required]
        [Column(Order = 1)]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public RolEmpleado Rol { get; set; }
        [Required]
        public bool Activo { get; set; } = false;
    }
}