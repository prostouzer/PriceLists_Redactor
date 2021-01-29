using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceLists_Redactor.Models.ViewModels
{
    public class PriceListViewModel
    {
        public PriceList PriceList { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}