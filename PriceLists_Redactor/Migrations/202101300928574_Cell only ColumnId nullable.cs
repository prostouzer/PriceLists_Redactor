namespace PriceLists_Redactor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CellonlyColumnIdnullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cells", "ItemId", "dbo.Items");
            DropIndex("dbo.Cells", new[] { "ItemId" });
            AlterColumn("dbo.Cells", "ItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.Cells", "ItemId");
            AddForeignKey("dbo.Cells", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cells", "ItemId", "dbo.Items");
            DropIndex("dbo.Cells", new[] { "ItemId" });
            AlterColumn("dbo.Cells", "ItemId", c => c.Int());
            CreateIndex("dbo.Cells", "ItemId");
            AddForeignKey("dbo.Cells", "ItemId", "dbo.Items", "Id");
        }
    }
}
