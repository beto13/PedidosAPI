namespace Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        #region Relations   
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        #endregion
    }
}
