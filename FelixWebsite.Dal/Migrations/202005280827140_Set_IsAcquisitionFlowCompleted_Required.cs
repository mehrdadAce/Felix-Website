namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Set_IsAcquisitionFlowCompleted_Required : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserInformations", "IsAcquisitionFlowCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInformations", "IsAcquisitionFlowCompleted", c => c.Boolean());
        }
    }
}
