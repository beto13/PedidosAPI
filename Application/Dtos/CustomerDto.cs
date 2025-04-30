using Domain.Entities;

namespace Application.Dtos
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}
