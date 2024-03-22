namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosTableUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "Activo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "Activo");
        }
    }
}
