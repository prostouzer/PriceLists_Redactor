using System.Data.Entity;
using PriceLists_Redactor.Models;

namespace PriceLists_Redactor.Data
{
    public interface IPriceListsRedactorContext
    {
        DbSet<PriceList> PriceLists { get; set; }
        DbSet<Item> Items { get; set; }
        DbSet<Column> Columns { get; set; }
        DbSet<Cell> Cells { get; set; }
        void MarkAsModified(PriceList priceList);
        void MarkAsModified(Column column);
        void MarkAsModified(Item item);
        void MarkAsModified(Cell cell);

        int SaveChanges();
        void Dispose();
    }
}