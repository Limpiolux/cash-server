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
        public DbSet<Formulario> Formularios { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<SubItem> SubItems { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
       
   
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubItem>()
         .HasRequired(s => s.Item)
         .WithMany(i => i.SubItems)
         .HasForeignKey(s => s.ItemId)
         .WillCascadeOnDelete(false); // Especificamos que no queremos que haya eliminación en cascada
        }
    }
}