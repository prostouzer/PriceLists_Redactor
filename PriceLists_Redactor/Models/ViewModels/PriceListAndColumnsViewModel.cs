using System.Collections.Generic;

namespace PriceLists_Redactor.Models.ViewModels
{
    public class PriceListAndColumnsViewModel
    {
        public PriceList PriceList { get; set; }
        public List<Column> Columns { get; set; }

        public PriceListAndColumnsViewModel(PriceList priceList)
        {
            PriceList = priceList;
            Columns = new List<Column>();
        }

        // для Edit'а - Без "беспараметрического конструктора" ругается
        public PriceListAndColumnsViewModel()
        {
            PriceList = new PriceList();
            Columns = new List<Column>();
        }
    }
}