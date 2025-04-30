namespace Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        #region Relations   
        public Order? Order { get; set; }
        public Product? Product { get; set; }
        #endregion
    }
}
