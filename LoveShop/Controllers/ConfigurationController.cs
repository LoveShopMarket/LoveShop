using Identity.Constants;
using LoveShop.DTOs.ConfigurationSetting;
using LoveShop.Models.ConfigurationSchema;
using LoveShop.Services.Contracts;
using Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LoveShop.Controllers
{
	[Authorize(Policy = Policies.RequireAdminRights)]
	[Route("api/[controller]")]
	[ApiController]
	public class ConfigurationController : ControllerBase
	{
		private readonly IConfigurationService _configurationService;

		public ConfigurationController(IConfigurationService configurationService)
		{
			_configurationService = configurationService;
		}

		[HttpGet]
		public async Task<ActionResult<Paginated<ConfigurationSettingDTO>>> GetConfigurationValuesAsync(
			int pageNumber = 0,
			int pageSize = 20,
			CancellationToken cancellationToken = default)
		{
			var paginatedFilter = new PaginatedFilter<ConfigurationSetting>(pageNumber, pageSize);

			var configurationSettingDTOs = await _configurationService.GetAllAsync(paginatedFilter, cancellationToken);

			return Ok(configurationSettingDTOs);
		}

		[HttpGet("{key}")]
		public async Task<ActionResult<ConfigurationSettingDTO>> GetConfigurationValueAsync(
			string key,
			CancellationToken cancellationToken = default)
		{
			var configurationSettingDTO = await _configurationService.GetAsync(key, cancellationToken);

			return Ok(configurationSettingDTO);
		}

		[HttpPost]
		public async Task<ActionResult> AddConfigurationValueAsync(
			ConfigurationSettingCreateDTO configurationSettingDTO,
			CancellationToken cancellationToken = default)
		{
			await _configurationService.CreateAsync(configurationSettingDTO, cancellationToken);

			return Ok();
		}

		[HttpPut]
		public async Task<ActionResult> SetConfigurationValueAsync(
			string key,
			JsonElement value,
			CancellationToken cancellationToken = default)
		{
			await _configurationService.SetAsync(key, value, cancellationToken);

			return Ok();
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteConfigurationSettingAsync(
			string key,
			CancellationToken cancellationToken = default)
		{
			await _configurationService.DeleteAsync(key, cancellationToken);
			return Ok();
		}
	}
}