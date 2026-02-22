using Identity.Models;

namespace Identity.Services
{
	public interface IUserConfirmationService
	{
		Task AddConfirmationRequestAsync(User user, CancellationToken cancellationToken = default);
	}
}