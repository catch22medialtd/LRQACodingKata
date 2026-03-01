using LRQACodingKata.Core.Constants;
using LRQACodingKata.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LRQACodingKata.Infrastructure.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(EntityStorageNames.Products);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(EntityPropertyLengths.Product_Name);

            builder.Property(x => x.Price)
                .HasPrecision(18, 2);
        }
    }
}