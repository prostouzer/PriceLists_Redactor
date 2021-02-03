using System.Data.Entity;
using PriceLists_Redactor.Models;

namespace PriceLists_Redactor.Data
{
    public class PriceListsRedactorContext : DbContext, IPriceListsRedactorContext
    {
        public PriceListsRedactorContext() : base("name=PriceLists_RedactorContext")
        {
        }

        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Cell> Cells { get; set; }

        public void MarkAsModified(PriceList priceList)
        {
            Entry(priceList).State = EntityState.Modified;
        }
        public void MarkAsModified(Column column)
        {
            Entry(column).State = EntityState.Modified;
        }
        public void MarkAsModified(Item item)
        {
            Entry(item).State = EntityState.Modified;
        }
        public void MarkAsModified(Cell cell)
        {
            Entry(cell).State = EntityState.Modified;
        }
    }
}
