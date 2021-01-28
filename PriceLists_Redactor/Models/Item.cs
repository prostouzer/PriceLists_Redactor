using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceLists_Redactor.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Cell> Cells { get; set; } = new List<Cell>();

        public int PriceListId { get; set; }
        public virtual PriceList PriceList { get; set; }
    }
}