using System.Text.Json;

namespace LoveShop.DTOs.ConfigurationSetting
{
	public sealed record ConfigurationSettingDTO(string Key, JsonElement Value, DateTimeOffset UpdatedAt);
}