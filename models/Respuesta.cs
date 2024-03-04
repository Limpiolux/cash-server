using cash_server.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public partial class Respuesta : IEntity
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int ItemId { get; set; }
        public int SubItemId { get; set; }

        public Item Item { get; set; }
        public SubItem SubItem { get; set; }

    }

    [MetadataType(typeof(RespuestaMetadata))]
    public partial class Respuesta
    {
        public class RespuestaMetadata
        {
            [Key]
            [Required]
            [Column(Order = 1)]
            public int Id { get; set; }

            [ForeignKey("Item")]
            [Required]
            [Column(Order = 2)]
            public int ItemId { get; set; }
            
            [ForeignKey("SubItem")]
            [Required]
            [Column(Order = 3)]
            public int SubItemId { get; set;}

            [StringLength(50)]
            public string Descripcion { get; set; }


        }

    }


}