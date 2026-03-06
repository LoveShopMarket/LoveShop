namespace Shared.DTOs.Category
{
	public sealed record CategoryUpdateDTO(
		Guid Id,
		string Name,
		Guid? ParentCategoryId);
}