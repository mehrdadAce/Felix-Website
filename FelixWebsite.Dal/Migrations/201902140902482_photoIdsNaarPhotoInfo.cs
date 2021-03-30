namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class photoIdsNaarPhotoInfo : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PhotoIds", newName: "PhotoInfoes");
            AlterTableAnnotations(
                "dbo.PhotoInfoes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MediaId = c.Int(nullable: false),
                    UserId = c.Int(nullable: false),
                    PhotoIdentifier = c.Int(nullable: false),
                    AuditDeleted = c.Boolean(nullable: false),
                    AuditDeletedBy = c.Int(),
                    AuditDeletedOn = c.DateTime(),
                },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_PhotoIds_IsDeleted",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    {
                        "DynamicFilter_PhotoInfo_IsDeleted",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
        }
        
        public override void Down()
        {
            AlterTableAnnotations(
                "dbo.PhotoInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediaId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        PhotoIdentifier = c.Int(nullable: false),
                        AuditDeleted = c.Boolean(nullable: false),
                        AuditDeletedBy = c.Int(),
                        AuditDeletedOn = c.DateTime(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_PhotoIds_IsDeleted",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_PhotoInfo_IsDeleted",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            RenameTable(name: "dbo.PhotoInfoes", newName: "PhotoIds");
        }
    }
}
