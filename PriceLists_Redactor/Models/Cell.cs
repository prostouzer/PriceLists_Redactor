using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PriceLists_Redactor.Models
{
    public class Cell
    {
        public int Id { get; set; }
        public string Data { get; set; }

        public int? ColumnId { get; set; }
        public virtual Column Column { get; set; }

        public int? ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}