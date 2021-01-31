using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceLists_Redactor.Models.ViewModels
{
    public class ItemAndCellsViewModel
    {
        public Item Item { get; set; }
        public IEnumerable<Cell> Cells { get; set; }
    }
}