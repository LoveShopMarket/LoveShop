using LoveShop.DTOs.ConfigurationSetting;
using LoveShop.Models.ConfigurationSchema;
using Shared;
using System.Text.Json;

namespace LoveShop.Services.Contracts
{
	public interface IConfigurationService
	{
		Task<Paginated<ConfigurationSettingDTO>> GetAllAsync(PaginatedFilter<ConfigurationSetting> paginatedFilter,
			CancellationToken cancellationToken);

		Task<ConfigurationSettingDTO?> GetAsync(string key, CancellationToken cancellationToken);

		Task SetAsync(string key, JsonElement value, CancellationToken cancellationToken);

		Task CreateAsync(ConfigurationSettingCreateDTO configurationSetting, CancellationToken cancellationToken);

		Task DeleteAsync(string key, CancellationToken cancellationToken);

		Task DeleteAsync(ConfigurationSetting configurationSetting, CancellationToken cancellationToken);
	}
}