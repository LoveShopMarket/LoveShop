using System.Linq.Expressions;

namespace Shared
{
	public sealed record Sort<T, TK>(Expression<Func<T, TK>> KeySelector);
}