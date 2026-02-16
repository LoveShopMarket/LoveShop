using LoveShop.DTOs.ProductImage;
using LoveShop.Models;
using LoveShop.Persistence;
using LoveShop.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LoveShop.Services
{
	public class ProductImageService : IProductImageService
	{
		private readonly ILogger<ProductImageService> _logger;
		private readonly LoveShopDbContext _loveShopDbContext;

		public ProductImageService(LoveShopDbContext context, ILogger<ProductImageService> logger)
		{
			_loveShopDbContext = context;
			_logger = logger;
		}

		public async Task AddAsync(
			ICollection<ProductImageUploadDTO> productImageDTOs,
			CancellationToken cancellationToken = default)
		{
			var products = await (
					from product in _loveShopDbContext.Products
					where productImageDTOs.Select(productImageDTO => productImageDTO.ProductId).Contains(product.Id)
					select product)
				.ToHashSetAsync(cancellationToken);

			var productImageAddresses = productImageDTOs
				.Select(productImageDTO => new ProductImageAddress
				{
					Product = products.Single(product => product.Id == productImageDTO.ProductId),
					ProductId = productImageDTO.ProductId,
					Address = productImageDTO.Image.FileName
				});

			await _loveShopDbContext.ProductImageAddress.AddRangeAsync(productImageAddresses, cancellationToken);

			await _loveShopDbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task<bool> DeleteAsync(
			Guid id,
			CancellationToken cancellationToken = default)
		{
			var productImageAddress = await _loveShopDbContext.ProductImageAddress.FindAsync([id], cancellationToken);

			if (productImageAddress is null)
			{
				return false;
			}

			return await DeleteAsync(productImageAddress, cancellationToken);
		}

		public async Task<bool> DeleteAsync(
			ProductImageAddress productImageAddress,
			CancellationToken cancellationToken = default)
		{
			try
			{
				_loveShopDbContext.ProductImageAddress.Remove(productImageAddress);

				await _loveShopDbContext.SaveChangesAsync(cancellationToken);

				return true;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Error occured while deleting product image: {ErrorMessage}",
					exception.Message);
				return false;
			}
		}
	}
}