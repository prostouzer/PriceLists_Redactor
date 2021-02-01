namespace PriceLists_Redactor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedColumnpropertiesfromCell : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cells", "ColumnId", "dbo.Columns");
            DropForeignKey("dbo.Cells", "ItemId", "dbo.Items");
            DropIndex("dbo.Cells", new[] { "ColumnId" });
            DropIndex("dbo.Cells", new[] { "ItemId" });
            AlterColumn("dbo.Cells", "ItemId", c => c.Int());
            CreateIndex("dbo.Cells", "ItemId");
            AddForeignKey("dbo.Cells", "ItemId", "dbo.Items", "Id");
            DropColumn("dbo.Cells", "ColumnId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cells", "ColumnId", c => c.Int());
            DropForeignKey("dbo.Cells", "ItemId", "dbo.Items");
            DropIndex("dbo.Cells", new[] { "ItemId" });
            AlterColumn("dbo.Cells", "ItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.Cells", "ItemId");
            CreateIndex("dbo.Cells", "ColumnId");
            AddForeignKey("dbo.Cells", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Cells", "ColumnId", "dbo.Columns", "Id");
        }
    }
}
