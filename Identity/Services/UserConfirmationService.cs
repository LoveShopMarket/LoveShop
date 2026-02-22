using Identity.Models;
using Identity.Persistence;

namespace Identity.Services
{
	public class UserConfirmationService : IUserConfirmationService
	{
		private readonly LoveShopIdentityDbContext _loveShopIdentityDbContext;

		public UserConfirmationService(LoveShopIdentityDbContext loveShopIdentityDbContext)
		{
			_loveShopIdentityDbContext = loveShopIdentityDbContext;
		}

		public async Task AddConfirmationRequestAsync(User user, CancellationToken cancellationToken = default)
		{
			var utcNow = DateTime.UtcNow;

			var outboxMessage = new OutboxMessage
			{
				OccurredOnUtc = utcNow,
				DeduplicationKey = $"user.id:{user.Id}",
				Type = "user.confirmation.request",
				Content = user.ToString()
			};
			await _loveShopIdentityDbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
		}
	}
}