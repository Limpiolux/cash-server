namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregoCampoCLiente2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisitaServicios", "Cliente2", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitaServicios", "Cliente2");
        }
    }
}
