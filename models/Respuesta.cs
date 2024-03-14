using cash_server.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace cash_server.Models
{
    public partial class Respuesta : IEntity
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int ItemId { get; set; }
       
        //marcar la propiedad Item de esta forma es para indicar que no queremos que esta propiedad se incluya en la respuesta JSON
        //que se envia desde la api, esto es para evitar referencias circulares
        [IgnoreDataMember]
        public Item Item { get; set; }

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
            
            [StringLength(250)]
            public string Descripcion { get; set; }


        }

    }


}