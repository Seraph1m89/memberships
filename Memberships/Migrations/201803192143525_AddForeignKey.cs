namespace Memberships.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKey : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Part", "Item_Id", "dbo.Item", "Id");
            AddForeignKey("dbo.Section", "Item_Id", "dbo.Item", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Part", "Item_Id", "dbo.Item");
            DropForeignKey("dbo.Section", "Item_Id", "dbo.Item");
        }
    }
}
