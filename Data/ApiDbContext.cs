using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity;
using cash_server.Models;
using cash_server.SharedKernel;

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
        public DbSet<Empleado> Empleados { get; set; }

        public DbSet<VisitaServicio> VisitaServicios  { get; set; }
        public DbSet<VisitaServicioForm> VisitaServicioForms { get; set; }

        public DbSet<UnidadNegocio> UnidadesNegocios { get; set; }

        public DbSet<ServicioPrestado> ServiciosPrestados { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubItem>()
                .HasRequired(s => s.Item)
                .WithMany(i => i.SubItems)
                .HasForeignKey(s => s.ItemId)
                .WillCascadeOnDelete(false); // Especificamos que si queremos que haya eliminación en cascada


            /*   modelBuilder.Entity<SubItem>()
           .HasRequired(s => s.Item)
           .WithMany() // No se especifica ninguna propiedad de navegación en Item, ya que no existe
           .HasForeignKey(s => s.ItemId)
           .WillCascadeOnDelete(false); // Especificamos que no queremos eliminación en cascada
           }*/

            modelBuilder.Entity<Empleado>()
            .Property(e => e.Rol)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnAnnotation("EnumType", typeof(RolEmpleado));

            modelBuilder.Entity<VisitaServicio>()
           .HasRequired(v => v.UnidadNegocio)
           .WithMany()
           .HasForeignKey(v => v.UnidadNegocioId)
           .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);


        }
    }
}

