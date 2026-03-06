namespace Shared.DTOs.Product
{
	public sealed record ProductDTO(
		Guid Id,
		string Name,
		string Description,
		decimal Price,
		ICollection<string> ImageAddresses,
		ICollection<Guid> CategoriesIds,
		byte[] RowVersion);
}