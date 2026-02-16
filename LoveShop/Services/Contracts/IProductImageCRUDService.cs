using LoveShop.DTOs.ProductImage;
using LoveShop.Models;

namespace LoveShop.Services.Contracts
{
	public interface IProductImageService
	{
		Task AddAsync(
			ICollection<ProductImageUploadDTO> productImageDTOs,
			CancellationToken cancellationToken = default);

		Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
		Task<bool> DeleteAsync(ProductImageAddress productImageAddress, CancellationToken cancellationToken = default);
	}
}