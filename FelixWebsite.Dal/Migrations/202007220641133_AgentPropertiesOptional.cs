namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgentPropertiesOptional : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInformations", "IsTakeOver", c => c.Boolean());
            AlterColumn("dbo.UserInformations", "TyreState", c => c.Int());
            AlterColumn("dbo.UserInformations", "Manual", c => c.Int());
            AlterColumn("dbo.UserInformations", "AgentName", c => c.String());
            AlterColumn("dbo.UserInformations", "AgentCity", c => c.String());
            AlterColumn("dbo.UserInformations", "AgentCellphone", c => c.String());
            AlterColumn("dbo.UserInformations", "AgentEmail", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInformations", "AgentEmail", c => c.String(nullable: false));
            AlterColumn("dbo.UserInformations", "AgentCellphone", c => c.String(nullable: false));
            AlterColumn("dbo.UserInformations", "AgentCity", c => c.String(nullable: false));
            AlterColumn("dbo.UserInformations", "AgentName", c => c.String(nullable: false));
            AlterColumn("dbo.UserInformations", "Manual", c => c.Int(nullable: false));
            AlterColumn("dbo.UserInformations", "TyreState", c => c.Int(nullable: false));
            DropColumn("dbo.UserInformations", "IsTakeOver");
        }
    }
}
