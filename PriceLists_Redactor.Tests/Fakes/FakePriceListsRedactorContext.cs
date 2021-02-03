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
        public FakePriceListsRedactorContext()
        {
            this.PriceLists = new FakePriceListsDbSet();
            this.Columns = new FakeColumnsDbSet();
            this.Items = new FakeItemsDbSet();
            this.Cells = new FakeCellsDbSet();
        }

        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Cell> Cells { get; set; }

        public void MarkAsModified(PriceList priceList) { }
        public void MarkAsModified(Column column) { }

        public void MarkAsModified(Item newItem)
        {
            var item = Items.Single(i => i.Id == newItem.Id);
            item.PriceList = newItem.PriceList;
            item.Id = newItem.Id;
            item.PriceListId = newItem.PriceListId;
            item.Title = newItem.Title;
        }

        public void MarkAsModified(Cell newCell)
        {
            var cell = Cells.Single(c => c.Id == newCell.Id);
            cell.Item = newCell.Item;
            cell.Data = newCell.Data;
            cell.Id = newCell.Id;
            cell.ItemId = newCell.Id;
        }

        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose() { }
    }
}
