using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity
{
	public class UserConfirmation : IUserConfirmation<User>
	{
		public Task<bool> IsConfirmedAsync(UserManager<User> manager, User user)
		{
			return Task.FromResult(user.ApprovalStatus == UserApprovalStatus.Approved);
		}
	}
}