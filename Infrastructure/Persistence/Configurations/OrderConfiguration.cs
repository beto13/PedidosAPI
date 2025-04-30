using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.TotalAmount)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(o => o.CreatedAt)
                   .IsRequired();

            builder.HasOne(o => o.Status)
                   .WithMany(s => s.Orders)
                   .HasForeignKey(o => o.OrderStatusId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
