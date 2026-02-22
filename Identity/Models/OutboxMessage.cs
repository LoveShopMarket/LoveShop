using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Models
{
	public sealed class OutboxMessage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; init; }

		public string Type { get; init; }
		public string Content { get; init; }
		public string DeduplicationKey { get; init; }
		public DateTime OccurredOnUtc { get; init; }
		public DateTime? ProcessedOnUtc { get; init; }
		public string? Error { get; init; }
	}

	public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
	{
		public void Configure(EntityTypeBuilder<OutboxMessage> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasIndex(x => x.DeduplicationKey)
				.IsUnique();
		}
	}
}