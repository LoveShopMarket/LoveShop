namespace Shared.DTOs.Category
{
	public sealed record CategoryDTO(
		Guid Id,
		string Name,
		Guid? ParentCategoryId,
		ICollection<Guid> ChildCategoriesIds);
}