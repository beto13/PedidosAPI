namespace Application.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderStatusId { get; set; }
        public decimal TotalAmount { get; set; }

        public IList<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
