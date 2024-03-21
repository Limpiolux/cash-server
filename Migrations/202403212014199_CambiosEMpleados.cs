namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosEMpleados : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Empleadoes", new[] { "Usuario_Id" });
            CreateIndex("dbo.Empleadoes", "Usuario_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Empleadoes", new[] { "Usuario_id" });
            CreateIndex("dbo.Empleadoes", "Usuario_Id");
        }
    }
}
