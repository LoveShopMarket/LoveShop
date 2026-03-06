using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace LoveShop.Models.ConfigurationSchema
{
	[Table("settings")]
	public class ConfigurationSetting
	{
		[Key] [MaxLength(150)] [Column("key")] public required string Key { get; init; }

		[Column("value")] public JsonElement Value { get; set; }

		[Column("updated_at")] public DateTimeOffset UpdatedAt { get; set; }
	}

	public class ConfigSettingConfiguration : IEntityTypeConfiguration<ConfigurationSetting>
	{
		public void Configure(EntityTypeBuilder<ConfigurationSetting> builder)
		{
			builder.HasKey(x => x.Key);

			builder.Property(x => x.Key)
				.HasMaxLength(150)
				.IsRequired();

			builder.Property(x => x.Value)
				.HasColumnType("jsonb")
				.IsRequired();

			builder.Property(x => x.UpdatedAt)
				.HasDefaultValueSql("now()");
		}
	}
}