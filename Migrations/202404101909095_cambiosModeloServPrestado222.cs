namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambiosModeloServPrestado222 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServicioPrestadoes", "ClienteNro", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServicioPrestadoes", "ClienteNro", c => c.Int(nullable: false));
        }
    }
}
