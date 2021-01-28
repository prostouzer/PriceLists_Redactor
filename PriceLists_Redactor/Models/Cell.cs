using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceLists_Redactor.Models
{
    public class Cell
    {
        public int Id { get; set; }
        public string Data { get; set; }

        public int ColumnId { get; set; }
        public virtual Column Column { get; set; }
    }
}