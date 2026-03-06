using LoveShop.Models.ConfigurationSchema;
using Microsoft.EntityFrameworkCore;

namespace LoveShop.Persistence
{
	public class ConfigurationDbContext : DbContext
	{
		public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options)
			: base(options)
		{
		}

		public DbSet<ConfigurationSetting> ConfigurationSettings { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.HasDefaultSchema("configuration");
		}
	}
}