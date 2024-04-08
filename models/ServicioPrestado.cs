using cash_server.Interfaces;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public class ServicioPrestado : IEntity
    {
        [Key]
        [Required]
        [Column(Order = 1)]
        public int Id { get; set; }
        //no obligatorio por las dudas, de todas formas este campo a nosotros no nos ingfluye
        public int CasaNro { get; set; }
 
        [Required]
        public string Nombre { get; set; }
        public UnidadNegocio UnidadNegocio { get; set; }
        
        [ForeignKey("UnidadNegocio")]
        [Required]
        [Column(Order = 2)]
        public int UnidadNegocioId { get; set; }
        //Activo = 1 esta activo, habilitado... Activo = 0 usuario dado de baja
        public bool Activo { get; set; } = false;
    }
}