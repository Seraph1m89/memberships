namespace Memberships.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserSubscription : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSubscription",
                c => new
                    {
                        SubscriptionId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        StartData = c.DateTime(),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.SubscriptionId, t.UserId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserSubscription");
        }
    }
}
