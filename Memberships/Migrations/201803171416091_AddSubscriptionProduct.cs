namespace Memberships.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubscriptionProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubscriptionProduct",
                c => new
                    {
                        SubscriptionId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SubscriptionId, t.ProductId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SubscriptionProduct");
        }
    }
}
