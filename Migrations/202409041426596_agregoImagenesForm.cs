namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agregoImagenesForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisitaServicioForms", "Imagen1", c => c.String());
            AddColumn("dbo.VisitaServicioForms", "Imagen2", c => c.String());
            AddColumn("dbo.VisitaServicioForms", "Imagen3", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitaServicioForms", "Imagen3");
            DropColumn("dbo.VisitaServicioForms", "Imagen2");
            DropColumn("dbo.VisitaServicioForms", "Imagen1");
        }
    }
}
