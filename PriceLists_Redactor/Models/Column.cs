using System.ComponentModel.DataAnnotations;

namespace PriceLists_Redactor.Models
{
    public class Column
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public string HeaderText { get; set; }
        [Required(ErrorMessage = "Выберите тип")]
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
        
        public int PriceListId { get; set; }
        public virtual PriceList PriceList { get; set; }
    }

    public enum DataType
    {
        SingleLine,
        MultiLine,
        Bool,
        Numeric
    }
}