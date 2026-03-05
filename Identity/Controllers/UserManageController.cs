using Identity.Constants;
using Identity.Models;
using Identity.Persistence;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.User;

namespace Identity.Controllers
{
	[Authorize(Policy = Policies.CanManageUsers)]
	[ApiController]
	[Route("api/identity/[controller]")]
	public class UserManageController : ControllerBase
	{
		private readonly IUserConfirmationService _userConfirmationService;

		private readonly UserManager<User> _userManager;

		public UserManageController(
			IUserConfirmationService userConfirmationService,
			UserManager<User> userManager)
		{
			_userConfirmationService = userConfirmationService;
			_userManager = userManager;
		}

		[HttpPost("addRoles")]
		public async Task AddToRolesAsync(Guid userId, string[] roles)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user is not null)
			{
				await _userManager.AddToRolesAsync(user, roles);
			}
		}

		[HttpGet("/user/pending")]
		public async Task<ActionResult<ICollection<UserDTO>>> GetPendingUserConfirmationRequests(
			CancellationToken cancellationToken = default)
		{
			var confirmationRequests =
				await _userConfirmationService.GetPendingConfirmationRequestsAsync(cancellationToken);

			return Ok(confirmationRequests);
		}

		[HttpPut("/user/approve")]
		public async Task<ActionResult> ConfirmUserConfirmationRequests(
			Guid[] userIds,
			CancellationToken cancellationToken = default)
		{
			var userDTOs = await _userConfirmationService.ConfirmUserConfirmationRequests(userIds, cancellationToken);

			return Ok(userDTOs);
		}

		[HttpPut("/user/reject")]
		public async Task<ActionResult> RejectUserConfirmationRequests(
			Guid[] userIds,
			CancellationToken cancellationToken = default)
		{
			var userDTOs = await _userConfirmationService.RejectUserConfirmationRequests(userIds, cancellationToken);

			return Ok(userDTOs);
		}
	}
}