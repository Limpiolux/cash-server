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
        //este no va a hacer obligatorio porque no es comun en ambas listas
        public int? ClienteNro { get; set; }
        //este no va a hacer obligatorio porque no es comun en ambas listas
        public string ClienteNombre { get; set; } = null;

        //le pongo string a este campo porue desde el otro sistema lo trae como string si no pincha
        [Required]
        public string CasaNro { get; set; }
        [Required]
        public string CasaNombre { get; set; }
        public string Localidad { get; set; } = null;
        public UnidadNegocio UnidadNegocio { get; set; }
        
        [ForeignKey("UnidadNegocio")]
        [Required]
        [Column(Order = 2)]
        public int UnidadNegocioId { get; set; }
        //Activo = 1 esta activo, habilitado... Activo = 0 usuario dado de baja
        public bool Activo { get; set; } = false;
    }
}