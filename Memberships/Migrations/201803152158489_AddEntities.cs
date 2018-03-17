namespace Memberships.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 2048),
                        Url = c.String(maxLength: 1024),
                        ImageUrl = c.String(maxLength: 1024),
                        Html = c.String(),
                        WaitDays = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ItemTypeId = c.Int(nullable: false),
                        PartId = c.Int(nullable: false),
                        SectionId = c.Int(nullable: false),
                        IsFree = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ItemType", "Item_Id", c => c.Int());
            AddColumn("dbo.ItemType", "Item_Id1", c => c.Int());
            AddColumn("dbo.ItemType", "Item_Id2", c => c.Int());
            CreateIndex("dbo.ItemType", "Item_Id");
            CreateIndex("dbo.ItemType", "Item_Id1");
            CreateIndex("dbo.ItemType", "Item_Id2");
            AddForeignKey("dbo.ItemType", "Item_Id", "dbo.Item", "Id");
            AddForeignKey("dbo.ItemType", "Item_Id1", "dbo.Item", "Id");
            AddForeignKey("dbo.ItemType", "Item_Id2", "dbo.Item", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemType", "Item_Id2", "dbo.Item");
            DropForeignKey("dbo.ItemType", "Item_Id1", "dbo.Item");
            DropForeignKey("dbo.ItemType", "Item_Id", "dbo.Item");
            DropIndex("dbo.ItemType", new[] { "Item_Id2" });
            DropIndex("dbo.ItemType", new[] { "Item_Id1" });
            DropIndex("dbo.ItemType", new[] { "Item_Id" });
            DropColumn("dbo.ItemType", "Item_Id2");
            DropColumn("dbo.ItemType", "Item_Id1");
            DropColumn("dbo.ItemType", "Item_Id");
            DropTable("dbo.Item");
        }
    }
}
