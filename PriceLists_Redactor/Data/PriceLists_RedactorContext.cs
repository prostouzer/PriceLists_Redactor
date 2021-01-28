using System.Data.Entity;
using PriceLists_Redactor.Models;

namespace PriceLists_Redactor.Data
{
    public class PriceLists_RedactorContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        public PriceLists_RedactorContext() : base("name=PriceLists_RedactorContext")
        {
        }

        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Cell> Cells { get; set; }
    }
}
