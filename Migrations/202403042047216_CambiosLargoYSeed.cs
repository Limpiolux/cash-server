namespace cash_server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosLargoYSeed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubItems", "Descripcion", c => c.String(maxLength: 250));
            AlterColumn("dbo.SubItems", "Comentario", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubItems", "Comentario", c => c.String(maxLength: 200));
            AlterColumn("dbo.SubItems", "Descripcion", c => c.String(maxLength: 50));
        }
    }
}
