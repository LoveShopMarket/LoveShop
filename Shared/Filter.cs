using System.Linq.Expressions;

namespace Shared
{
	public sealed record Filter<T>(
		PaginatedFilter<T> PaginatedFilter,
		Expression<Func<T, bool>>? Condition = null);
}