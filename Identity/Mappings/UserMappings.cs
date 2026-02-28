using Identity.Models;
using Shared.DTOs;

namespace Identity.Mappings
{
	public static class UserMappings
	{
		public static UserDTO ToDTO(this User user)
		{
			return new UserDTO(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty);
		}
	}
}