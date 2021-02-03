using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceLists_Redactor.Data;
using PriceLists_Redactor.Models;

namespace PriceLists_Redactor.Tests.Fakes
{
    class FakePriceListsRedactorContext : IPriceListsRedactorContext
    {
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Cell> Cells { get; set; }

        public void MarkAsModified(PriceList priceList) { }
        public void MarkAsModified(Column column) { }
        public void MarkAsModified(Item item) { }
        public void MarkAsModified(Cell cell) { }

        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose() { }
    }
}
