namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DamageIsSelected : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SchemeDamages", "IsSelected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SchemeDamages", "IsSelected");
        }
    }
}
