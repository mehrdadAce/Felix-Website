namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgentProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInformations", "AgentName", c => c.String(nullable: false));
            AddColumn("dbo.UserInformations", "AgentCity", c => c.String(nullable: false));
            AddColumn("dbo.UserInformations", "AgentCellphone", c => c.String(nullable: false));
            AddColumn("dbo.UserInformations", "AgentEmail", c => c.String(nullable: false));
            AddColumn("dbo.UserInformations", "Insurance", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInformations", "Insurance");
            DropColumn("dbo.UserInformations", "AgentEmail");
            DropColumn("dbo.UserInformations", "AgentCellphone");
            DropColumn("dbo.UserInformations", "AgentCity");
            DropColumn("dbo.UserInformations", "AgentName");
        }
    }
}
