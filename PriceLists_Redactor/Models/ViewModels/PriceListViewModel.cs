using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceLists_Redactor.Models.ViewModels
{
    public class PriceListViewModel
    {
        public PriceList PriceList { get; set; }
        public IEnumerable<Column> Columns { get; set; }

        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Cell> Cells { get; set; }

        public PriceListViewModel(PriceList priceList, IEnumerable<Column> columns, IEnumerable<Item> items, IEnumerable<Cell> cells)
        {
            PriceList = priceList;
            Columns = columns;
            Items = items;
            Cells = cells;
        }
    }
}