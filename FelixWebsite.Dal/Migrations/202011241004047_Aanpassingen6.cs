namespace FelixWebsite.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Aanpassingen6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SchemeDamages", "HeavyDamage", c => c.Boolean());
            AddColumn("dbo.SchemeDamages", "Dent", c => c.Boolean());
            AddColumn("dbo.UserInformations", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.UserInformations", "Firstname", c => c.String());
            AlterColumn("dbo.UserInformations", "Lastname", c => c.String());
            DropColumn("dbo.SchemeDamages", "ClientCosts");
            DropColumn("dbo.SchemeDamages", "Costs");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SchemeDamages", "Costs", c => c.Int());
            AddColumn("dbo.SchemeDamages", "ClientCosts", c => c.Boolean());
            AlterColumn("dbo.UserInformations", "Lastname", c => c.String(nullable: false));
            AlterColumn("dbo.UserInformations", "Firstname", c => c.String(nullable: false));
            DropColumn("dbo.UserInformations", "Name");
            DropColumn("dbo.SchemeDamages", "Dent");
            DropColumn("dbo.SchemeDamages", "HeavyDamage");
        }
    }
}
