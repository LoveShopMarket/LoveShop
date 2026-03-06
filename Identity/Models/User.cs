using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Models
{
	public class User : IdentityUser<Guid>
	{
		public UserApprovalStatus ApprovalStatus { get; set; } = UserApprovalStatus.Pending;
	}

	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.Property(u => u.ApprovalStatus)
				.HasConversion(
					v => v.Value,
					v => UserApprovalStatus.Create(v))
				.HasDefaultValue(UserApprovalStatus.Pending);
		}
	}

	public sealed class UserApprovalStatus
	{
		public string Value { get; }

		public static UserApprovalStatus Create(string value)
		{
			ArgumentNullException.ThrowIfNull(value);

			return value switch
			{
				_ when value.Equals("Pending", StringComparison.InvariantCultureIgnoreCase)
					=> Pending,

				_ when value.Equals("Approved", StringComparison.InvariantCultureIgnoreCase)
					=> Approved,

				_ when value.Equals("Rejected", StringComparison.InvariantCultureIgnoreCase)
					=> Rejected,

				_ => throw new ArgumentException($"Unknown status value: {value}")
			};
		}

		private UserApprovalStatus(string value)
		{
			Value = value;
		}

		public static readonly UserApprovalStatus Pending =
			new("Pending");

		public static readonly UserApprovalStatus Approved =
			new("Approved");

		public static readonly UserApprovalStatus Rejected =
			new("Rejected");
	}
}