using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveShop.Models
{
	[Table("product_images")]
	public class ProductImageAddress : BaseEntity
	{
		[Column("image_address")] public string Address { get; set; }

		[Column("product_id")] public Guid ProductId { get; set; }

		public Product Product { get; set; }
	}

	public class ProductImageAddressConfiguration : BaseEntityConfiguration<ProductImageAddress>
	{
		public override void Configure(EntityTypeBuilder<ProductImageAddress> builder)
		{
			base.Configure(builder);

			builder.HasOne(productImageAddress => productImageAddress.Product)
				.WithMany(p => p.ProductImageAddresses)
				.HasConstraintName("FK_product_images_products_product_id")
				.HasForeignKey(productImageAddress => productImageAddress.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(productImageAddress => productImageAddress.Address)
				.HasMaxLength(300);

			builder.HasIndex(productImageAddress => productImageAddress.Address)
				.IsUnique();
		}
	}
}