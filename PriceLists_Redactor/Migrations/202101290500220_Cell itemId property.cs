namespace PriceLists_Redactor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CellitemIdproperty : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cells", "Item_Id", "dbo.Items");
            DropIndex("dbo.Cells", new[] { "Item_Id" });
            RenameColumn(table: "dbo.Cells", name: "Item_Id", newName: "ItemId");
            AlterColumn("dbo.Cells", "ItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.Cells", "ItemId");
            AddForeignKey("dbo.Cells", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cells", "ItemId", "dbo.Items");
            DropIndex("dbo.Cells", new[] { "ItemId" });
            AlterColumn("dbo.Cells", "ItemId", c => c.Int());
            RenameColumn(table: "dbo.Cells", name: "ItemId", newName: "Item_Id");
            CreateIndex("dbo.Cells", "Item_Id");
            AddForeignKey("dbo.Cells", "Item_Id", "dbo.Items", "Id");
        }
    }
}
