namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreaciondeUniNegocioYServPrestado : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServicioPrestadoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnidadNegocioId = c.Int(nullable: false),
                        CasaNro = c.Int(nullable: false),
                        Nombre = c.String(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServicioPrestadoes", "UnidadNegocioId", "dbo.UnidadNegocios");
            DropIndex("dbo.ServicioPrestadoes", new[] { "UnidadNegocioId" });
            DropTable("dbo.UnidadNegocios");
            DropTable("dbo.ServicioPrestadoes");
        }
    }
}
