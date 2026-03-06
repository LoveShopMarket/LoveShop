using Identity.Models;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.User;

namespace Identity.Services
{
	public class UserConfirmationService : IUserConfirmationService
	{
		private readonly LoveShopIdentityDbContext _loveShopIdentityDbContext;

		public UserConfirmationService(LoveShopIdentityDbContext loveShopIdentityDbContext)
		{
			_loveShopIdentityDbContext = loveShopIdentityDbContext;
		}

		public async Task<ICollection<UserDTO>> GetPendingConfirmationRequestsAsync(
			CancellationToken cancellationToken = default)
		{
			return await _loveShopIdentityDbContext.Users
				.Where(x => x.ApprovalStatus == UserApprovalStatus.Pending)
				.Select(user => new UserDTO(user.Id, user.Email ?? string.Empty))
				.ToListAsync(cancellationToken);
		}

		public async Task<ICollection<UserDTO>> ConfirmUserConfirmationRequests(ICollection<Guid> userIds,
			CancellationToken cancellationToken = default)
		{
			var userDTOs = new List<UserDTO>();
			var users = await _loveShopIdentityDbContext.Users.Where(u => userIds.Contains(u.Id))
				.ToListAsync(cancellationToken);
			foreach (var user in users)
			{
				user.ApprovalStatus = UserApprovalStatus.Approved;
				userDTOs.Add(new UserDTO(user.Id, user.Email ?? string.Empty));
			}

			await _loveShopIdentityDbContext.SaveChangesAsync(cancellationToken);

			return userDTOs;
		}

		public async Task<ICollection<UserDTO>> RejectUserConfirmationRequests(ICollection<Guid> userIds,
			CancellationToken cancellationToken = default)
		{
			var userDTOs = new List<UserDTO>();
			var users = await _loveShopIdentityDbContext.Users.Where(u => userIds.Contains(u.Id))
				.ToListAsync(cancellationToken);
			foreach (var user in users)
			{
				user.ApprovalStatus = UserApprovalStatus.Rejected;
				userDTOs.Add(new UserDTO(user.Id, user.Email ?? string.Empty));
			}

			await _loveShopIdentityDbContext.SaveChangesAsync(cancellationToken);

			return userDTOs;
		}
	}
}