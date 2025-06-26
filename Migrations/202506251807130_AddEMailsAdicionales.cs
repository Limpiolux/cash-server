namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEMailsAdicionales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisitaServicios", "EmailsAdicionales", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitaServicios", "EmailsAdicionales");
        }
    }
}
