using cash_server.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public partial class Item : IEntity
    {
    public int Id { get; set; }
    public int FormId { get; set; }
    public string Descripcion { get; set; }

    public Formulario Formulario { get; set; }
    public IList<SubItem> SubItems { get; private set; }
    }

    [MetadataType(typeof(ItemMetadata))]
    public partial class Item
    {
        public class ItemMetadata
        {
            [Key]
            [Required]
            [Column(Order = 1)]
            public int Id { get; set; }

            [ForeignKey("Formulario")]
            [Required]
            [Column(Order = 2)]
            public int FormId { get; set; }

            [StringLength(250)]
            public string Descripcion { get; set; }


        }

    }


}