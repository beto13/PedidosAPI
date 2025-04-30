using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasIndex(c => c.Email).IsUnique();

            builder.Property(c => c.PhoneNumber)
                   .HasMaxLength(20);

            builder.HasMany(c => c.Orders)
                   .WithOne(o => o.Customer!)
                   .HasForeignKey(o => o.CustomerId);
        }
    }
}
