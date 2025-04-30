namespace Domain.Entities
{
    public class OrderStatus
    {
        public int Id { get; set; }           
        public string Name { get; set; } = null!;

        #region Relations   
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        #endregion
    }
}
