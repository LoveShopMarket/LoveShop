using Identity.Models;
using Shared.DTOs.OutboxMessage;

namespace Identity.Mappings
{
	public static class OutboxMessageMappings
	{
		public static OutboxMessageDTO ToDTO(this OutboxMessage outboxMessage)
		{
			return new OutboxMessageDTO(
				outboxMessage.Id,
				outboxMessage.Type,
				outboxMessage.Content,
				outboxMessage.DeduplicationKey,
				outboxMessage.OccurredOnUtc,
				outboxMessage.ProcessedOnUtc,
				outboxMessage.Error);
		}
	}
}