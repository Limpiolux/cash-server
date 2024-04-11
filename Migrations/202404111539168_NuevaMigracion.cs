namespace cash_server.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class NuevaMigracion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Empleadoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Rol = c.Int(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "EnumType",
                                    new AnnotationValues(oldValue: null, newValue: "cash_server.SharedKernel.RolEmpleado")
                                },
                            }),
                        Usuario_id = c.Int(),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_id)
                .Index(t => t.Usuario_id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Mail = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Rol = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Formularios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Numero = c.Int(nullable: false),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Formularios", t => t.FormId, cascadeDelete: true)
                .Index(t => t.FormId);
            
            CreateTable(
                "dbo.SubItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 250),
                        Comentario = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Respuestas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.ServicioPrestadoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnidadNegocioId = c.Int(nullable: false),
                        ClienteNro = c.Int(),
                        ClienteNombre = c.String(),
                        CasaNro = c.String(nullable: false),
                        CasaNombre = c.String(nullable: false),
                        Localidad = c.String(),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UnidadNegocios", t => t.UnidadNegocioId, cascadeDelete: true)
                .Index(t => t.UnidadNegocioId);
            
            CreateTable(
                "dbo.UnidadNegocios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Cuit = c.String(),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VisitaServicioForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VisitaId = c.Int(nullable: false),
                        FormId = c.Int(nullable: false),
                        Item = c.String(nullable: false),
                        SubItem = c.String(nullable: false),
                        Comentario = c.String(),
                        Respuesta = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Formularios", t => t.FormId, cascadeDelete: true)
                .ForeignKey("dbo.VisitaServicios", t => t.VisitaId, cascadeDelete: true)
                .Index(t => t.VisitaId)
                .Index(t => t.FormId);
            
            CreateTable(
                "dbo.VisitaServicios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnidadNegocioId = c.Int(nullable: false),
                        ServicioPrestadoId = c.Int(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                        Cliente = c.String(nullable: false, maxLength: 50),
                        FechaVisita = c.DateTime(nullable: false),
                        ModeloVehiculo = c.String(maxLength: 50),
                        Conductor = c.String(maxLength: 50),
                        TipoVehiculoId = c.Int(),
                        Dominio = c.String(maxLength: 50),
                        Proveedor = c.String(maxLength: 50),
                        SupervisorId = c.Int(nullable: false),
                        Fecha_operacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServicioPrestadoes", t => t.ServicioPrestadoId, cascadeDelete: true)
                .ForeignKey("dbo.Empleadoes", t => t.SupervisorId, cascadeDelete: true)
                .ForeignKey("dbo.UnidadNegocios", t => t.UnidadNegocioId)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.UnidadNegocioId)
                .Index(t => t.ServicioPrestadoId)
                .Index(t => t.UsuarioId)
                .Index(t => t.SupervisorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisitaServicioForms", "VisitaId", "dbo.VisitaServicios");
            DropForeignKey("dbo.VisitaServicios", "UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.VisitaServicios", "UnidadNegocioId", "dbo.UnidadNegocios");
            DropForeignKey("dbo.VisitaServicios", "SupervisorId", "dbo.Empleadoes");
            DropForeignKey("dbo.VisitaServicios", "ServicioPrestadoId", "dbo.ServicioPrestadoes");
            DropForeignKey("dbo.VisitaServicioForms", "FormId", "dbo.Formularios");
            DropForeignKey("dbo.ServicioPrestadoes", "UnidadNegocioId", "dbo.UnidadNegocios");
            DropForeignKey("dbo.Respuestas", "ItemId", "dbo.Items");
            DropForeignKey("dbo.SubItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "FormId", "dbo.Formularios");
            DropForeignKey("dbo.Empleadoes", "Usuario_id", "dbo.Usuarios");
            DropIndex("dbo.VisitaServicios", new[] { "SupervisorId" });
            DropIndex("dbo.VisitaServicios", new[] { "UsuarioId" });
            DropIndex("dbo.VisitaServicios", new[] { "ServicioPrestadoId" });
            DropIndex("dbo.VisitaServicios", new[] { "UnidadNegocioId" });
            DropIndex("dbo.VisitaServicioForms", new[] { "FormId" });
            DropIndex("dbo.VisitaServicioForms", new[] { "VisitaId" });
            DropIndex("dbo.ServicioPrestadoes", new[] { "UnidadNegocioId" });
            DropIndex("dbo.Respuestas", new[] { "ItemId" });
            DropIndex("dbo.SubItems", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "FormId" });
            DropIndex("dbo.Empleadoes", new[] { "Usuario_id" });
            DropTable("dbo.VisitaServicios");
            DropTable("dbo.VisitaServicioForms");
            DropTable("dbo.UnidadNegocios");
            DropTable("dbo.ServicioPrestadoes");
            DropTable("dbo.Respuestas");
            DropTable("dbo.SubItems");
            DropTable("dbo.Items");
            DropTable("dbo.Formularios");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Empleadoes",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "Rol",
                        new Dictionary<string, object>
                        {
                            { "EnumType", "cash_server.SharedKernel.RolEmpleado" },
                        }
                    },
                });
        }
    }
}
