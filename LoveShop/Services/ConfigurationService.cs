using LoveShop.DTOs.ConfigurationSetting;
using LoveShop.Extensions;
using LoveShop.Models.ConfigurationSchema;
using LoveShop.Persistence;
using LoveShop.Services.Contracts;
using Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LoveShop.Services
{
	public class ConfigurationService : IConfigurationService
	{
		private readonly ConfigurationDbContext _configurationDbContext;

		public ConfigurationService(ConfigurationDbContext configurationDbContext)
		{
			_configurationDbContext = configurationDbContext;
		}

		public async Task<Paginated<ConfigurationSettingDTO>> GetAllAsync(
			PaginatedFilter<ConfigurationSetting> paginatedFilter,
			CancellationToken cancellationToken)
		{
			var configurationSettings = await _configurationDbContext.ConfigurationSettings
				.AsNoTracking()
				.Paginate(paginatedFilter)
				.Select(x => new ConfigurationSettingDTO(x.Key, x.Value, x.UpdatedAt))
				.ToListAsync(cancellationToken);

			var paginatedResult = new Paginated<ConfigurationSettingDTO>(
				configurationSettings,
				paginatedFilter.PageNumber,
				Math.Min(configurationSettings.Count, paginatedFilter.PageSize),
				configurationSettings.Count);

			return paginatedResult;
		}

		public async Task<ConfigurationSettingDTO?> GetAsync(string key, CancellationToken cancellationToken)
		{
			var configSetting = await _configurationDbContext.ConfigurationSettings
				.AsNoTracking()
				.SingleOrDefaultAsync(x => x.Key == key, cancellationToken);

			if (configSetting is null)
			{
				return null;
			}

			var configSettingDTO =
				new ConfigurationSettingDTO(configSetting.Key, configSetting.Value, configSetting.UpdatedAt);

			return configSettingDTO;
		}

		public async Task SetAsync(string key, JsonElement value, CancellationToken cancellationToken)
		{
			var configSetting = await _configurationDbContext.ConfigurationSettings
				.SingleOrDefaultAsync(x => x.Key == key, cancellationToken);

			// TODO: привести в норм состояние
			if (configSetting is null)
			{
				throw new KeyNotFoundException();
			}

			configSetting.Value = value;

			await _configurationDbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task CreateAsync(ConfigurationSettingCreateDTO configurationSettingDTO,
			CancellationToken cancellationToken)
		{
			var configSetting = new ConfigurationSetting
			{
				Key = configurationSettingDTO.Key,
				Value = configurationSettingDTO.Value,
				UpdatedAt = DateTimeOffset.UtcNow
			};

			_configurationDbContext.ConfigurationSettings.Add(configSetting);
			await _configurationDbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task DeleteAsync(string key, CancellationToken cancellationToken)
		{
			var configSetting = await _configurationDbContext.ConfigurationSettings
				.SingleOrDefaultAsync(x => x.Key == key, cancellationToken);

			if (configSetting is not null)
			{
				await DeleteAsync(configSetting, cancellationToken);
			}
		}

		public async Task DeleteAsync(ConfigurationSetting configurationSetting, CancellationToken cancellationToken)
		{
			_configurationDbContext.ConfigurationSettings.Remove(configurationSetting);
			await _configurationDbContext.SaveChangesAsync(cancellationToken);
		}
	}
}