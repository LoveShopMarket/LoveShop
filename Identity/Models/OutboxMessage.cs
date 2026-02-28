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

		public required string Type { get; init; }
		public required string Content { get; init; }
		public required string DeduplicationKey { get; init; }
		public required DateTime OccurredOnUtc { get; init; }
		public DateTime? ProcessedOnUtc { get; set; }
		public string? Error { get; set; }
	}

	public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
	{
		public void Configure(EntityTypeBuilder<OutboxMessage> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasIndex(x => x.DeduplicationKey)
				.IsUnique();

			builder.Property(x => x.Type)
				.HasMaxLength(300)
				.IsRequired();

			builder.Property(x => x.Content)
				.HasColumnType("jsonb");

			builder.Property(x => x.DeduplicationKey)
				.HasMaxLength(300)
				.IsRequired();

			builder.Property(x => x.OccurredOnUtc)
				.IsRequired();

			builder.Property(x => x.Error).HasMaxLength(500);
		}
	}
}