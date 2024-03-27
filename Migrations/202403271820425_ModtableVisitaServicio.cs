namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModtableVisitaServicio : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VisitaServicios", "PreventorId", "dbo.Empleadoes");
            DropIndex("dbo.VisitaServicios", new[] { "PreventorId" });
            DropColumn("dbo.VisitaServicios", "PreventorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VisitaServicios", "PreventorId", c => c.Int(nullable: false));
            CreateIndex("dbo.VisitaServicios", "PreventorId");
            AddForeignKey("dbo.VisitaServicios", "PreventorId", "dbo.Empleadoes", "Id", cascadeDelete: true);
        }
    }
}
