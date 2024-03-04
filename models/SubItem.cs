using cash_server.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public partial class SubItem: IEntity
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Descripcion { get; set; }
        public string Comentario { get; set; }

        public Item Item { get; set; }

    }

    [MetadataType(typeof(SubItemMetadata))]
    public partial class SubItem
    {
        public class SubItemMetadata
        {
            [Key]
            [Required]
            [Column(Order = 1)]
            public int Id { get; set; }

            [ForeignKey("Item")]
            [Required]
            public int ItemId { get; set; }

            [StringLength(250)]
            public string Descripcion { get; set; }

            [StringLength(250)]
            public string Comentario { get; set; }

          
        }

    }
}