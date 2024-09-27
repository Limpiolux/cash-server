using cash_server.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public class UpdateVisitaServicioDto
    {
        //esta clase es solo para poder guardar los datos a modificar en la visita servicio, cuando se le quiere agregar lo del vehiculo
        public int Id { get; set; }  // El ID del registro que se va a modificar
        public string Conductor { get; set; }
        public string Dominio { get; set; }
        public string ModeloVehiculo { get; set; }
        public TipoVehiculo? TipoVehiculoId { get; set; }
        public string Proveedor { get; set; }
    }
}