using System.Collections.Generic;

namespace PriceLists_Redactor.Models.ViewModels
{
    public class ItemAndCellsViewModel
    {
        public Item Item { get; set; }
        public IEnumerable<Cell> Cells { get; set; }
    }
}