using Identity.Constants;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.OutboxMessage;

namespace Identity.Controllers
{
	[Authorize(Policy = Policies.CanManageUsers)]
	[ApiController]
	[Route("api/identity/[controller]")]
	public class UserManageController : ControllerBase
	{
		private readonly IUserConfirmationService _userConfirmationService;

		public UserManageController(IUserConfirmationService userConfirmationService)
		{
			_userConfirmationService = userConfirmationService;
		}

		[HttpGet("/unprocessed")]
		public async Task<ActionResult<ICollection<OutboxMessageDTO>>> GetUnprocessedUserConfirmationRequests(
			CancellationToken cancellationToken = default)
		{
			var confirmationRequests =
				await _userConfirmationService.GetUnprocessedConfirmationRequestsAsync(cancellationToken);

			return Ok(confirmationRequests);
		}
	}
}