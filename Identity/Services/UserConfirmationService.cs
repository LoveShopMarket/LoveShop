using Identity.Constants;
using Identity.Models;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.OutboxMessage;
using System.Text.Json;

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
				Type = OutboxMessageTypes.UserConfirmationRequest,
				Content = JsonSerializer.Serialize(user)
			};
			await _loveShopIdentityDbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
		}

		public async Task<ICollection<OutboxMessageDTO>> GetUnprocessedConfirmationRequestsAsync(
			CancellationToken cancellationToken = default)
		{
			var confirmationRequests = await _loveShopIdentityDbContext.OutboxMessages
				.Where(x => x.Type == OutboxMessageTypes.UserConfirmationRequest
				            && x.ProcessedOnUtc == null)
				.Select(x => x.ToDTO())
				.ToListAsync(cancellationToken);

			return confirmationRequests;
		}
	}
}