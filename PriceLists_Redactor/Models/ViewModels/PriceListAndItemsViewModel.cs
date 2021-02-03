using System.Collections.Generic;

namespace PriceLists_Redactor.Models.ViewModels
{
    public class PriceListAndItemsViewModel
    {
        public PriceList PriceList { get; set; }
        public List<Column> Columns { get; set; }

        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Cell> Cells { get; set; }

        public PriceListAndItemsViewModel(PriceList priceList, List<Column> columns, IEnumerable<Item> items, IEnumerable<Cell> cells)
        {
            PriceList = priceList;
            Columns = columns;
            Items = items;
            Cells = cells;
        }
    }
}