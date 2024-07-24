using cash_server.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
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
        [IgnoreDataMember] //para que no muestre cuando traigo todas las visitas... no me muestre en la respuesta del json
        public VisitaServicio VisitaServicio { get; set; }
        [IgnoreDataMember]
        public Formulario Formulario { get; set; }

        //se agrega este campo para saber que version del formulario completó el usuario.
        [Required]
        public int version { get; set; }

        public string ComentarioGeneral { get; set; } = null;

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
            [Required]
            public int version { get; set; }
            public string ComentarioGeneral { get; set; } = null;
        
        }
    }
}