using LoveShop.DTOs.ProductImage;
using LoveShop.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoveShop.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class ProductImageController : ControllerBase
	{
		private readonly IProductImageService _productImageService;

		public ProductImageController(IProductImageService productImageService)
		{
			_productImageService = productImageService;
		}

		[HttpPost]
		public async Task<ActionResult> AddImagesAsync(
			Guid id,
			[FromForm] IFormFile image,
			//[FromForm] ICollection<IFormFile> images,
			CancellationToken cancellationToken = default)
		{
			var list = new List<ProductImageUploadDTO> { new(id, image) };
			//var productImageDTOs = images.Select(image => new ProductImageDTO(id, image)).ToArray();

			await _productImageService.AddAsync(list, cancellationToken);

			return Created();
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteImagesAsync(
			Guid id,
			CancellationToken cancellationToken = default)
		{
			await _productImageService.DeleteAsync(id, cancellationToken);

			return Ok();
		}
	}
}