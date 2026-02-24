namespace Shared.DTOs.Category
{
	public sealed record CategoryCreateDTO(
		string Name,
		Guid? ParentCategoryId);
}