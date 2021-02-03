namespace PriceLists_Redactor.Models
{
    public class Cell
    {
        public int Id { get; set; }
        public string Data { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}