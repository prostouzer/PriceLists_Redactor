namespace PriceLists_Redactor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CellsItemIdnonnullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cells", "ColumnId", "dbo.Columns");
            DropForeignKey("dbo.Cells", "Item_Id", "dbo.Items");
            DropIndex("dbo.Cells", new[] { "ColumnId" });
            DropIndex("dbo.Cells", new[] { "Item_Id" });
            RenameColumn(table: "dbo.Cells", name: "Item_Id", newName: "ItemId");
            AlterColumn("dbo.Cells", "ItemId", c => c.Int(nullable: false));
            AlterColumn("dbo.Columns", "HeaderText", c => c.String(nullable: false));
            AlterColumn("dbo.PriceLists", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Items", "Title", c => c.String(nullable: false));
            CreateIndex("dbo.Cells", "ItemId");
            AddForeignKey("dbo.Cells", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            DropColumn("dbo.Cells", "ColumnId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cells", "ColumnId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Cells", "ItemId", "dbo.Items");
            DropIndex("dbo.Cells", new[] { "ItemId" });
            AlterColumn("dbo.Items", "Title", c => c.String());
            AlterColumn("dbo.PriceLists", "Name", c => c.String());
            AlterColumn("dbo.Columns", "HeaderText", c => c.String());
            AlterColumn("dbo.Cells", "ItemId", c => c.Int());
            RenameColumn(table: "dbo.Cells", name: "ItemId", newName: "Item_Id");
            CreateIndex("dbo.Cells", "Item_Id");
            CreateIndex("dbo.Cells", "ColumnId");
            AddForeignKey("dbo.Cells", "Item_Id", "dbo.Items", "Id");
            AddForeignKey("dbo.Cells", "ColumnId", "dbo.Columns", "Id", cascadeDelete: true);
        }
    }
}
