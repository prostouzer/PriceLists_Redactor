namespace PriceLists_Redactor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cells",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.String(),
                        ColumnId = c.Int(nullable: false),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Columns", t => t.ColumnId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .Index(t => t.ColumnId)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        PriceListId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceLists", t => t.PriceListId, cascadeDelete: true)
                .Index(t => t.PriceListId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "PriceListId", "dbo.PriceLists");
            DropForeignKey("dbo.Cells", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.Cells", "ColumnId", "dbo.Columns");
            DropIndex("dbo.Items", new[] { "PriceListId" });
            DropIndex("dbo.Cells", new[] { "Item_Id" });
            DropIndex("dbo.Cells", new[] { "ColumnId" });
            DropTable("dbo.Items");
            DropTable("dbo.Cells");
        }
    }
}
