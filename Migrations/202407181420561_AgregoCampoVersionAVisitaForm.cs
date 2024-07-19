namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregoCampoVersionAVisitaForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisitaServicioForms", "version", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitaServicioForms", "version");
        }
    }
}
