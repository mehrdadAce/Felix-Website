namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsInsuredChassisNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInformations", "IsInsured", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserInformations", "Chassisnumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInformations", "Chassisnumber");
            DropColumn("dbo.UserInformations", "IsInsured");
        }
    }
}
