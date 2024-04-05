namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosUltimos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisitaServicios", "Fecha_operacion", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitaServicios", "Fecha_operacion");
        }
    }
}
