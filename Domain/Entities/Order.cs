namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public int OrderStatusId { get; set; }


        #region Relations
        public OrderStatus? Status { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        #endregion
    }
}
