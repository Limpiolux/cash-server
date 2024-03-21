using cash_server.Interfaces;
using cash_server.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public class Usuario: IEntity
    {
        [Key]
        [Required]
        [Column(Order = 1)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public RolUsuario Rol { get; set; } 
    }
}