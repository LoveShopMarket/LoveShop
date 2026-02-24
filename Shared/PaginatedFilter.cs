namespace Shared
{
	public sealed record PaginatedFilter<T>(
		int PageNumber,
		int PageSize);
}