namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendedBasicTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhotoIds", "PhotoIdentifier", c => c.Int(nullable: false));
            AddColumn("dbo.UserInformations", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInformations", "Remarks");
            DropColumn("dbo.PhotoIds", "PhotoIdentifier");
        }
    }
}
