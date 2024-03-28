using cash_server.Interfaces;
using cash_server.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static cash_server.Models.Item;

namespace cash_server.Models
{
    public partial class VisitaServicio : IEntity
    {
        public int Id { get; set; }
        public string ServicioPrestado { get; set; }
        public string Cliente { get; set; }
        public string UnidadNegocio { get; set; }
        public DateTime FechaVisita { get; set; }
        public string ModeloVehiculo { get; set; } = null;
        public string Conductor { get; set; } = null;
        public TipoVehiculo? TipoVehiculoId { get; set; }
        public string Dominio { get; set; } = null;
        public string Proveedor { get; set; } = null;
        
        public int SupervisorId { get; set; }
        //public int PreventorId { get; set; }
        public Empleado Supervisor { get; set; }
        //public Empleado Preventor { get; set; }

        public int UsuarioId { get; set; }
      

        public Usuario Usuario { get; set; }
        public List<VisitaServicioForm> Formularios { get; set; }
    }


    [MetadataType(typeof(VisitaServicioMetadata))]
    public partial class VisitaServicio
    {
        public class VisitaServicioMetadata
        {
            [Key]
            [Column(Order = 1)]
            public int Id { get; set; }

            [Required]
            [StringLength(50)]
            public string ServicioPrestado { get; set; }

            [Required]
            [StringLength(50)]
            public string Cliente { get; set; }

            [Required]
            [StringLength(50)]
            public string UnidadNegocio { get; set; }

            [Required]
            public DateTime FechaVisita { get; set; }

            [StringLength(50)]
            public string ModeloVehiculo { get; set; } = null;

            [StringLength(50)]
            public string Conductor { get; set; } = null;
            [StringLength(50)]
            public string Dominio { get; set; } = null;
            [StringLength(50)]
            public string Proveedor { get; set; } = null;

            //Propiedades de navegación para las relaciones con Empleado
            [ForeignKey("SupervisorId")]
            [Column(Order = 2)]
            public Empleado Supervisor { get; set; }

            /*[ForeignKey("PreventorId")]
            [Required]
            [Column(Order = 3)]
            public Empleado Preventor { get; set; }*/

            [ForeignKey("Usuario")]
            [Column(Order = 4)]
            public int UsuarioId { get; set; }


        }
    }

}