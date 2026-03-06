using System.Text.Json;

namespace LoveShop.DTOs.ConfigurationSetting
{
	public sealed record ConfigurationSettingCreateDTO(string Key, JsonElement Value);
}