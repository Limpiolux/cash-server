namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambioTableUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RolUsuarios", "Rol_Id", "dbo.RolUsuarios");
            DropForeignKey("dbo.RolUsuarios", "Usuario_Id", "dbo.Usuarios");
            DropIndex("dbo.RolUsuarios", new[] { "Rol_Id" });
            DropIndex("dbo.RolUsuarios", new[] { "Usuario_Id" });
            AddColumn("dbo.Usuarios", "Rol", c => c.Int(nullable: false));
            DropTable("dbo.RolUsuarios");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RolUsuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rol_Id = c.Int(nullable: false),
                        Usuario_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Usuarios", "Rol");
            CreateIndex("dbo.RolUsuarios", "Usuario_Id");
            CreateIndex("dbo.RolUsuarios", "Rol_Id");
            AddForeignKey("dbo.RolUsuarios", "Usuario_Id", "dbo.Usuarios", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RolUsuarios", "Rol_Id", "dbo.RolUsuarios", "Id");
        }
    }
}
