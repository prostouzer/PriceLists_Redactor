namespace PriceLists_Redactor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedrequiredattributestomodels : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Columns", "HeaderText", c => c.String(nullable: false));
            AlterColumn("dbo.PriceLists", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Items", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "Title", c => c.String());
            AlterColumn("dbo.PriceLists", "Name", c => c.String());
            AlterColumn("dbo.Columns", "HeaderText", c => c.String());
        }
    }
}
