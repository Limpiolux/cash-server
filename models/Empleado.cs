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
    public partial class Empleado : IEntity
    {    //les puse requeridas porque los supervisores se van a cargar de un endpoint
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

        //este creo que no sería obligatorio
        public Usuario Usuario { get; set; } = null;

        [ForeignKey("Usuario")]
        public int? Usuario_id { get; set; }
        //para saber el supervisor, a que unidad de negocio pertenece si limpio, distmaster, tyt etc
        //para los empleados que sean preventores este campo sera null
        public UnidadNegocio UnidadNegocio { get; set; } = null;
        
        [ForeignKey("UnidadNegocio")]
        public int? UnidadNegocio_id { get; set; }
        [Required]
        public bool Activo { get; set; } = false;

    }

}