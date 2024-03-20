namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosvisitasUltimo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VisitaServicioForms", "Comentario", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VisitaServicioForms", "Comentario", c => c.String(nullable: false));
        }
    }
}
