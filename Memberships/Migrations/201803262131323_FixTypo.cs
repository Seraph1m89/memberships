namespace Memberships.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTypo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSubscription", "StartDate", c => c.DateTime());
            DropColumn("dbo.UserSubscription", "StartData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserSubscription", "StartData", c => c.DateTime());
            DropColumn("dbo.UserSubscription", "StartDate");
        }
    }
}
