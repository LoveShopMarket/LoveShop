using Identity.Models;
using Shared.DTOs.OutboxMessage;

namespace Identity.Services
{
	public interface IUserConfirmationService
	{
		Task AddConfirmationRequestAsync(User user, CancellationToken cancellationToken = default);

		Task<ICollection<OutboxMessageDTO>> GetUnprocessedConfirmationRequestsAsync(
			CancellationToken cancellationToken = default);
	}
}