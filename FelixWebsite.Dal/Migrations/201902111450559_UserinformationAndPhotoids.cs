namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class UserinformationAndPhotoids : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhotoIds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediaId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        AuditDeleted = c.Boolean(nullable: false),
                        AuditDeletedBy = c.Int(),
                        AuditDeletedOn = c.DateTime(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PhotoIds_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Firstname = c.String(nullable: false),
                        Lastname = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Gsm = c.String(nullable: false),
                        LicensePlate = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        WheelStateInternal = c.Int(nullable: false),
                        Wheelstate = c.Int(nullable: false),
                        ManualInternal = c.Int(nullable: false),
                        Manual = c.Int(nullable: false),
                        AuditDeleted = c.Boolean(nullable: false),
                        AuditDeletedBy = c.Int(),
                        AuditDeletedOn = c.DateTime(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserInformation_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserInformations",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserInformation_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PhotoIds",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PhotoIds_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
