namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormulariosItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Formularios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Numero = c.Int(nullable: false),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Formularios", t => t.FormId, cascadeDelete: true)
                .Index(t => t.FormId);
            
            CreateTable(
                "dbo.SubItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 50),
                        Comentario = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Respuestas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        SubItemId = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.SubItems", t => t.SubItemId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.SubItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Respuestas", "SubItemId", "dbo.SubItems");
            DropForeignKey("dbo.Respuestas", "ItemId", "dbo.Items");
            DropForeignKey("dbo.SubItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "FormId", "dbo.Formularios");
            DropIndex("dbo.Respuestas", new[] { "SubItemId" });
            DropIndex("dbo.Respuestas", new[] { "ItemId" });
            DropIndex("dbo.SubItems", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "FormId" });
            DropTable("dbo.Respuestas");
            DropTable("dbo.SubItems");
            DropTable("dbo.Items");
            DropTable("dbo.Formularios");
        }
    }
}
