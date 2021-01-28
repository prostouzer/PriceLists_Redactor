namespace PriceLists_Redactor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class columnforeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Columns", "PriceList_Id", "dbo.PriceLists");
            DropIndex("dbo.Columns", new[] { "PriceList_Id" });
            RenameColumn(table: "dbo.Columns", name: "PriceList_Id", newName: "PriceListId");
            AlterColumn("dbo.Columns", "PriceListId", c => c.Int(nullable: false));
            CreateIndex("dbo.Columns", "PriceListId");
            AddForeignKey("dbo.Columns", "PriceListId", "dbo.PriceLists", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Columns", "PriceListId", "dbo.PriceLists");
            DropIndex("dbo.Columns", new[] { "PriceListId" });
            AlterColumn("dbo.Columns", "PriceListId", c => c.Int());
            RenameColumn(table: "dbo.Columns", name: "PriceListId", newName: "PriceList_Id");
            CreateIndex("dbo.Columns", "PriceList_Id");
            AddForeignKey("dbo.Columns", "PriceList_Id", "dbo.PriceLists", "Id");
        }
    }
}
