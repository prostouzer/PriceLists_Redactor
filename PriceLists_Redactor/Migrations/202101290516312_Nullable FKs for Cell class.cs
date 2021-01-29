namespace PriceLists_Redactor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableFKsforCellclass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cells", "ColumnId", "dbo.Columns");
            DropIndex("dbo.Cells", new[] { "ColumnId" });
            RenameColumn(table: "dbo.Cells", name: "Item_Id", newName: "ItemId");
            RenameIndex(table: "dbo.Cells", name: "IX_Item_Id", newName: "IX_ItemId");
            AlterColumn("dbo.Cells", "ColumnId", c => c.Int());
            CreateIndex("dbo.Cells", "ColumnId");
            AddForeignKey("dbo.Cells", "ColumnId", "dbo.Columns", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cells", "ColumnId", "dbo.Columns");
            DropIndex("dbo.Cells", new[] { "ColumnId" });
            AlterColumn("dbo.Cells", "ColumnId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Cells", name: "IX_ItemId", newName: "IX_Item_Id");
            RenameColumn(table: "dbo.Cells", name: "ItemId", newName: "Item_Id");
            CreateIndex("dbo.Cells", "ColumnId");
            AddForeignKey("dbo.Cells", "ColumnId", "dbo.Columns", "Id", cascadeDelete: true);
        }
    }
}
