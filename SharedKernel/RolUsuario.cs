using cash_server.Interfaces;
using cash_server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cash_server.SharedKernel
{
    public enum RolUsuario
    {
        Preventor = 1,
        Administrador = 2
    }
}