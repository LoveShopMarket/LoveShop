using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence
{
	public class LoveShopIdentityDbContext : IdentityDbContext<User, Role, Guid>
	{
		public LoveShopIdentityDbContext(DbContextOptions<LoveShopIdentityDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.HasDefaultSchema("identity");

			builder.ApplyConfiguration(new UserConfiguration());
		}
	}
}