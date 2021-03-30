namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class SchemeDamage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SchemeDamages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SchemeEntry = c.Int(nullable: false),
                        Damage = c.Boolean(),
                        Marks = c.Boolean(),
                        HailDamage = c.Boolean(),
                        ClientCosts = c.Boolean(),
                        Remarks = c.String(),
                        Costs = c.Int(),
                        AuditDeleted = c.Boolean(nullable: false),
                        AuditDeletedBy = c.Int(),
                        AuditDeletedOn = c.DateTime(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SchemeDamage_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SchemeDamages",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SchemeDamage_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
