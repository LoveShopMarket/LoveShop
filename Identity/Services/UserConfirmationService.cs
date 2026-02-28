using Identity.Constants;
using Identity.Mappings;
using Identity.Models;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
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

		public async Task AddConfirmationRequestAsync(UserDTO userDTO, CancellationToken cancellationToken = default)
		{
			var utcNow = DateTime.UtcNow;

			var outboxMessage = new OutboxMessage
			{
				OccurredOnUtc = utcNow,
				DeduplicationKey = $"{OutboxMessageDeduplicationKeys.UserId}:{userDTO.Id}",
				Type = OutboxMessageTypes.UserConfirmationRequest,
				Content = JsonSerializer.Serialize(userDTO)
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

		public async Task ProcessUserConfirmationRequestAsync(Guid id, CancellationToken cancellationToken = default)
		{
			var confirmationRequest = await _loveShopIdentityDbContext.OutboxMessages
				.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

			if (confirmationRequest is not null)
			{
				var userDTO = JsonSerializer.Deserialize<UserDTO>(confirmationRequest.Content)!;
				var user = await _loveShopIdentityDbContext.Users.SingleOrDefaultAsync(
					x => x.Id == userDTO.Id, cancellationToken);
				if (user is not null)
				{
					user.EmailConfirmed = true;

					confirmationRequest.ProcessedOnUtc = DateTime.UtcNow;

					await _loveShopIdentityDbContext.SaveChangesAsync(cancellationToken);
				}
			}
		}
	}
}