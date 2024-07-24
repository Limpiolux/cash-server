namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agregoCampoComentarioGeneral : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisitaServicioForms", "ComentarioGeneral", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitaServicioForms", "ComentarioGeneral");
        }
    }
}
