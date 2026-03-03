using Shared.DTOs.User;

namespace Identity.Services
{
	public interface IUserConfirmationService
	{
		Task<ICollection<UserDTO>> GetPendingConfirmationRequestsAsync(
			CancellationToken cancellationToken = default);

		Task<ICollection<UserDTO>> ConfirmUserConfirmationRequests(ICollection<Guid> userIds,
			CancellationToken cancellationToken = default);

		Task<ICollection<UserDTO>> RejectUserConfirmationRequests(ICollection<Guid> userIds,
			CancellationToken cancellationToken = default);
	}
}