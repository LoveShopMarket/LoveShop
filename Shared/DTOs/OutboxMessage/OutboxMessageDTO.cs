namespace Shared.DTOs.OutboxMessage
{
	public sealed record OutboxMessageDTO(
		Guid Id,
		string Type,
		string Content,
		string DeduplicationKey,
		DateTime OccuredOnUtc,
		DateTime? ProcessedOnUtc,
		string? Error);
}