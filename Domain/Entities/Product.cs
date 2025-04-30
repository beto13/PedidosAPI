namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        #region Relations   
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        #endregion
    }
}
