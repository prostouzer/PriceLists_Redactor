using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceLists_Redactor.Models.ViewModels
{
    public class ItemAndCellsViewModel
    {
        public Item Item { get; set; }
        public IEnumerable<Column> Columns { get; set; }

        public ItemAndCellsViewModel(Item item)
        {
            Item = item;
            Columns = new List<Column>();
        }
    }
}