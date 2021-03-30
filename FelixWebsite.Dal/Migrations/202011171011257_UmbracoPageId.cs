namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UmbracoPageId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInformations", "UmbracoPageId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInformations", "UmbracoPageId");
        }
    }
}
