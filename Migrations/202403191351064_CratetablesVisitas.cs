namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CratetablesVisitas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VisitaServicioForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VisitaId = c.Int(nullable: false),
                        FormId = c.Int(nullable: false),
                        Item = c.String(nullable: false),
                        SubItem = c.String(nullable: false),
                        Comentario = c.String(nullable: false),
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
                        UsuarioId = c.Int(nullable: false),
                        ServicioPrestado = c.String(nullable: false, maxLength: 50),
                        Cliente = c.String(nullable: false, maxLength: 50),
                        UnidadNegocio = c.String(nullable: false, maxLength: 50),
                        FechaVisita = c.DateTime(nullable: false),
                        ModeloVehiculo = c.String(maxLength: 50),
                        Conductor = c.String(maxLength: 50),
                        TipoVehiculoId = c.Int(),
                        Dominio = c.String(maxLength: 50),
                        Proveedor = c.String(maxLength: 50),
                        SupervisorId = c.Int(nullable: false),
                        PreventorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empleadoes", t => t.PreventorId, cascadeDelete: false) //los pongo en false, porque si no tira error, que hay borrado en cascada y tira error al correr la migracion
                .ForeignKey("dbo.Empleadoes", t => t.SupervisorId, cascadeDelete: false)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioId, cascadeDelete: false)
                .Index(t => t.UsuarioId)
                .Index(t => t.SupervisorId)
                .Index(t => t.PreventorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisitaServicioForms", "VisitaId", "dbo.VisitaServicios");
            DropForeignKey("dbo.VisitaServicios", "UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.VisitaServicios", "SupervisorId", "dbo.Empleadoes");
            DropForeignKey("dbo.VisitaServicios", "PreventorId", "dbo.Empleadoes");
            DropForeignKey("dbo.VisitaServicioForms", "FormId", "dbo.Formularios");
            DropIndex("dbo.VisitaServicios", new[] { "PreventorId" });
            DropIndex("dbo.VisitaServicios", new[] { "SupervisorId" });
            DropIndex("dbo.VisitaServicios", new[] { "UsuarioId" });
            DropIndex("dbo.VisitaServicioForms", new[] { "FormId" });
            DropIndex("dbo.VisitaServicioForms", new[] { "VisitaId" });
            DropTable("dbo.VisitaServicios");
            DropTable("dbo.VisitaServicioForms");
        }
    }
}
