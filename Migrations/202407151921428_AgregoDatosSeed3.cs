namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregoDatosSeed3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Formularios", "version", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Formularios", "version");
        }
    }
}
