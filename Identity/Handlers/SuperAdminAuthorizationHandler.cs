using Identity.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Handlers
{
	public class SuperAdminAuthorizationHandler : IAuthorizationHandler
	{
		public Task HandleAsync(AuthorizationHandlerContext context)
		{
			if (context.User.IsInRole(Roles.SuperAdmin) || context.User.IsInRole(Roles.SoftwareDeveloper))
			{
				foreach (var requirement in context.Requirements)
				{
					context.Succeed(requirement);
				}
			}

			return Task.CompletedTask;
		}
	}
}