namespace Memberships.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateItemProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ItemType", "Item_Id2", "dbo.Item");
            DropForeignKey("dbo.ItemType", "Item_Id1", "dbo.Item");
            DropIndex("dbo.ItemType", new[] { "Item_Id2" });
            DropIndex("dbo.ItemType", new[] { "Item_Id1" });
            DropColumn("dbo.ItemType", "Item_Id1");
            DropColumn("dbo.ItemType", "Item_Id2");
            AddColumn("dbo.Part", "Item_Id", c => c.Int());
            AddColumn("dbo.Section", "Item_Id", c => c.Int());
            CreateIndex("dbo.Part", "Item_Id");
            CreateIndex("dbo.Section", "Item_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ItemType", "Item_Id2", c => c.Int());
            AddColumn("dbo.ItemType", "Item_Id1", c => c.Int());
            DropIndex("dbo.Section", new[] { "Item_Id" });
            DropIndex("dbo.Part", new[] { "Item_Id" });
            RenameColumn(table: "dbo.Section", name: "Item_Id", newName: "Item_Id2");
            RenameColumn(table: "dbo.Part", name: "Item_Id", newName: "Item_Id1");
            CreateIndex("dbo.ItemType", "Item_Id2");
            CreateIndex("dbo.ItemType", "Item_Id1");
        }
    }
}
