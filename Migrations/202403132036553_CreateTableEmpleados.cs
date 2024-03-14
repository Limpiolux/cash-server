namespace cash_server.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableEmpleados : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Empleadoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Rol = c.Int(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "EnumType",
                                    new AnnotationValues(oldValue: null, newValue: "cash_server.SharedKernel.RolEmpleado")
                                },
                            }),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Empleadoes",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "Rol",
                        new Dictionary<string, object>
                        {
                            { "EnumType", "cash_server.SharedKernel.RolEmpleado" },
                        }
                    },
                });
        }
    }
}
