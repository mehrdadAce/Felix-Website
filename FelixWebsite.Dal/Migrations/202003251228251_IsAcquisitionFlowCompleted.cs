namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsAcquisitionFlowCompleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInformations", "IsAcquisitionFlowCompleted", c => c.Boolean());
            Sql(
            @"
                BEGIN TRY
	                BEGIN TRANSACTION
	
	                UPDATE [dbo].[UserInformations]
	                SET [IsAcquisitionFlowCompleted] = 1
	                
	                COMMIT TRANSACTION 
                END TRY
                BEGIN CATCH  
                    ROLLBACK TRANSACTION
                END CATCH 
            ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInformations", "IsAcquisitionFlowCompleted");
        }
    }
}
