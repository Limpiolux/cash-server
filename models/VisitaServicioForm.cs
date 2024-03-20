using cash_server.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public partial class VisitaServicioForm : IEntity
    {
        public int Id { get; set; }
        public int VisitaId { get; set; }
        public int FormId { get; set; }
        public string Item { get; set; }
        public string SubItem { get; set; }
        public string Comentario { get; set; }
        public string Respuesta { get; set; }

        public VisitaServicio VisitaServicio { get; set; }

        public Formulario Formulario { get; set; }

    }

    [MetadataType(typeof(VisitaServicioFormMetadata))]
    public partial class VisitaServicioForm
    {
        public class VisitaServicioFormMetadata
        {
            [Required]
            [Column(Order = 1)]
            public int Id { get; set; }

            [ForeignKey("VisitaServicio")]
            [Required]
            [Column(Order = 2)]
            public int VisitaId { get; set; }
            
            [ForeignKey("Formulario")]
            [Required]
            [Column(Order = 3)]
            public int FormId { get; set; }
            [Required]
            public string Item { get; set; }
            [Required]
            public string SubItem { get; set; }
            public string Comentario { get; set; } = null;
            [Required]
            public string Respuesta { get; set; }
        }
    }
}