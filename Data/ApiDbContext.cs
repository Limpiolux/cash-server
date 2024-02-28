using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity;
using cash_server.Models;

namespace cash_server.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext() :
           base("DefaultConnection")
        {

        }

        public DbSet<Usuario> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //en caso q haya relaciones se pueden conf aca
        }
    }
}