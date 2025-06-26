namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampoUnidadNegocioEn_Empleados : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empleadoes", "UnidadNegocio_id", c => c.Int());
            CreateIndex("dbo.Empleadoes", "UnidadNegocio_id");
            AddForeignKey("dbo.Empleadoes", "UnidadNegocio_id", "dbo.UnidadNegocios", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Empleadoes", "UnidadNegocio_id", "dbo.UnidadNegocios");
            DropIndex("dbo.Empleadoes", new[] { "UnidadNegocio_id" });
            DropColumn("dbo.Empleadoes", "UnidadNegocio_id");
        }
    }
}
