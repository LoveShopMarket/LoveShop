using Identity.Constants;
using Identity.Handlers;
using Identity.Models;
using Identity.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddIdentity(this IServiceCollection services, string? connectionString)
		{
			services.AddDbContext<LoveShopIdentityDbContext>(options =>
				options.UseNpgsql(connectionString));

			services.AddAuthentication();

			services.AddAuthorizationCore(options =>
				{
					options.AddPolicy(Policies.RequireAdminRights,
						policy => policy.RequireRole(Roles.SuperAdmin, Roles.SoftwareDeveloper));

					options.AddPolicy(Policies.RequireAccessToCart,
						policy => policy.RequireRole(Roles.User));

					options.AddPolicy(Policies.CanEditProducts,
						policy => policy.RequireRole(Roles.ProductAdmin));
					options.AddPolicy(Policies.CanRemoveProducts,
						policy => policy.RequireRole(Roles.ProductAdmin));

					options.AddPolicy(Policies.CanEditCategories,
						policy => policy.RequireRole(Roles.CategoryAdmin));
					options.AddPolicy(Policies.CanRemoveCategories,
						policy => policy.RequireRole(Roles.CategoryAdmin));

					options.AddPolicy(Policies.CanManageUsers,
						policy => policy.RequireRole(Roles.UserAdmin));
				})
				.AddIdentityCore<User>(options =>
				{
					options.User.RequireUniqueEmail = true;

					options.SignIn.RequireConfirmedEmail = true;

					options.Password.RequireDigit = false;
					options.Password.RequireLowercase = false;
					options.Password.RequireUppercase = false;
					options.Password.RequireNonAlphanumeric = false;
				})
				.AddRoles<Role>()
				.AddEntityFrameworkStores<LoveShopIdentityDbContext>()
				.AddDefaultTokenProviders();

			services.AddSingleton<IAuthorizationHandler, SuperAdminAuthorizationHandler>();

			services.AddControllers();

			return services;
		}
	}
}