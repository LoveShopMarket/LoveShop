using Shared.DTOs;
using Shared.DTOs.OutboxMessage;

namespace Identity.Services
{
	public interface IUserConfirmationService
	{
		Task AddConfirmationRequestAsync(UserDTO userDTO, CancellationToken cancellationToken = default);

		Task<ICollection<OutboxMessageDTO>> GetUnprocessedConfirmationRequestsAsync(
			CancellationToken cancellationToken = default);

		Task ProcessUserConfirmationRequestAsync(
			Guid id,
			CancellationToken cancellationToken = default);
	}
}