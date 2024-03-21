namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambioTableUser2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Usuarios", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Usuarios", "Mail", c => c.String(nullable: false));
            AlterColumn("dbo.Usuarios", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuarios", "Password", c => c.String());
            AlterColumn("dbo.Usuarios", "Mail", c => c.String());
            AlterColumn("dbo.Usuarios", "Name", c => c.String());
        }
    }
}
