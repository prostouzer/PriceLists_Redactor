using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceLists_Redactor.Models
{
    public class Column
    {
        public int Id { get; set; }
        public string HeaderText { get; set; }
        public DataType DataType { get; set; }
        public int DataLength {
            get
            {
                switch (DataType)
                {
                    case DataType.SingleLine:
                        return 20;
                    case DataType.MultiLine:
                        return 150;
                    case DataType.Bool:
                        return 1;
                    case DataType.Numeric:
                        return 10;
                }
                return -1;
            }
        }
    }

    public enum DataType
    {
        SingleLine,
        MultiLine,
        Bool,
        Numeric
    }
}