namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambiosModeloServPrestado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServicioPrestadoes", "Localidad", c => c.String());
            AlterColumn("dbo.ServicioPrestadoes", "ClienteNombre", c => c.String());
            AlterColumn("dbo.ServicioPrestadoes", "CasaNro", c => c.String(nullable: false));
            AlterColumn("dbo.ServicioPrestadoes", "CasaNombre", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServicioPrestadoes", "CasaNombre", c => c.String());
            AlterColumn("dbo.ServicioPrestadoes", "CasaNro", c => c.String());
            AlterColumn("dbo.ServicioPrestadoes", "ClienteNombre", c => c.String(nullable: false));
            DropColumn("dbo.ServicioPrestadoes", "Localidad");
        }
    }
}
