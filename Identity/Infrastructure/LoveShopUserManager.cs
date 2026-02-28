using Identity.Mappings;
using Identity.Models;
using Identity.Persistence;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure
{
	public class LoveShopUserManager : UserManager<User>
	{
		private readonly IUserConfirmationService _userConfirmationService;
		private readonly LoveShopIdentityDbContext _loveShopIdentityDbContext;

		public LoveShopUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
			IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
			IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
			IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger,
			LoveShopIdentityDbContext loveShopIdentityDbContext, IUserConfirmationService userConfirmationService)
			: base(store, optionsAccessor, passwordHasher, userValidators,
				passwordValidators, keyNormalizer, errors, services, logger)
		{
			_loveShopIdentityDbContext = loveShopIdentityDbContext;
			_userConfirmationService = userConfirmationService;
		}

		public override async Task<IdentityResult> CreateAsync(User user, string password)
		{
			await using var transaction = await _loveShopIdentityDbContext.Database.BeginTransactionAsync();
			try
			{
				var identityResult = await base.CreateAsync(user, password);
				if (!identityResult.Succeeded)
				{
					await transaction.RollbackAsync();
					return identityResult;
				}

				await _userConfirmationService.AddConfirmationRequestAsync(user.ToDTO());
				await _loveShopIdentityDbContext.SaveChangesAsync();

				await transaction.CommitAsync();

				return identityResult;
			}
			catch
			{
				await transaction.RollbackAsync();
				throw;
			}
		}
	}
}