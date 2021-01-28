namespace PriceLists_Redactor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PriceLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Columns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderText = c.String(),
                        DataType = c.Int(nullable: false),
                        PriceList_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceLists", t => t.PriceList_Id)
                .Index(t => t.PriceList_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Columns", "PriceList_Id", "dbo.PriceLists");
            DropIndex("dbo.Columns", new[] { "PriceList_Id" });
            DropTable("dbo.Columns");
            DropTable("dbo.PriceLists");
        }
    }
}
