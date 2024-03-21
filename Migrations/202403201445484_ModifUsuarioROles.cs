namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifUsuarioROles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RolUsuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rol_Id = c.Int(nullable: false),
                        Usuario_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RolUsuarios", t => t.Rol_Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id, cascadeDelete: true)
                .Index(t => t.Rol_Id)
                .Index(t => t.Usuario_Id);
            
            AddColumn("dbo.Empleadoes", "Usuario_Id", c => c.Int());
            CreateIndex("dbo.Empleadoes", "Usuario_Id");
            AddForeignKey("dbo.Empleadoes", "Usuario_Id", "dbo.Usuarios", "Id");
            DropColumn("dbo.Usuarios", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "Role", c => c.String());
            DropForeignKey("dbo.Empleadoes", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.RolUsuarios", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.RolUsuarios", "Rol_Id", "dbo.RolUsuarios");
            DropIndex("dbo.RolUsuarios", new[] { "Usuario_Id" });
            DropIndex("dbo.RolUsuarios", new[] { "Rol_Id" });
            DropIndex("dbo.Empleadoes", new[] { "Usuario_Id" });
            DropColumn("dbo.Empleadoes", "Usuario_Id");
            DropTable("dbo.RolUsuarios");
        }
    }
}
