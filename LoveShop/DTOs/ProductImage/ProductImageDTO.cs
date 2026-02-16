namespace LoveShop.DTOs.ProductImage
{
	public record ProductImageDTO(Guid ProductId, IFormFile Image, byte[] RowVersion);

	public record ProductImageUploadDTO(Guid ProductId, IFormFile Image);
}