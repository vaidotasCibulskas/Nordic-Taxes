using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Domain.Services;
using Nordic.Taxes.Domain.Services.Communication;
using Nordic.Taxes.Extensions;
using Nordic.Taxes.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nordic.Taxes.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class TaxesController : ControllerBase
	{
		private readonly ILogger<TaxesController> _logger;
		private readonly ITaxService _taxService;
		private readonly IMapper _mapper;


		public TaxesController(ILogger<TaxesController> logger, ITaxService taxService, IMapper mapper)
		{
			_logger = logger;
			_taxService = taxService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IEnumerable<TaxResource>> GetAllAsync()
		{
			var taxes = await _taxService.ListAsync();
			var resources = _mapper.Map<IEnumerable<Tax>, IEnumerable<TaxResource>>(taxes);
			
			return resources;
		}

		[HttpGet("getTax/{municId}/{date}")]
		public async Task<IActionResult> GetMunicipalityTax(int municId, DateTime date)
		{
			var result = await _taxService.GetMunicipalityTaxOfDay(municId, date);
			if (!result.Success)
				return BadRequest(result.Message);

			var taxResource = _mapper.Map<Tax, SpecificDateTaxResource>(result.Tax);
			return Ok(taxResource);
		}

		[HttpPost("AddYearly")]
		public async Task<IActionResult> PostAsync([FromBody] SaveYearlyTaxResource resource)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState.GetErrorMessages());
			var tax = _mapper.Map<SaveYearlyTaxResource, Tax>(resource);
			return await SaveTaxes(tax);
		}

		[HttpPost("AddMonthly")]
		public async Task<IActionResult> PostAsync([FromBody] SaveMonthlyTaxResource resource)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState.GetErrorMessages());
			var tax = _mapper.Map<SaveMonthlyTaxResource, Tax>(resource);
			return await SaveTaxes(tax);
		}

		[HttpPost("AddWeekly")]
		public async Task<IActionResult> PostAsync([FromBody] SaveWeeklyTaxResource resource)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState.GetErrorMessages());
			var tax = _mapper.Map<SaveWeeklyTaxResource, Tax>(resource);
			return await SaveTaxes(tax);
		}

		[HttpPost("AddDaily")]
		public async Task<IActionResult> PostAsync([FromBody] SaveDailyTaxResource resource)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState.GetErrorMessages());
			var tax = _mapper.Map<SaveDailyTaxResource, Tax>(resource);
			return await SaveTaxes(tax);
		}


		private async Task<IActionResult> SaveTaxes(Tax tax)
		{
			var result = await _taxService.SaveAsync(tax);
			return ReturnResult(result);
		}

		private IActionResult ReturnResult(TaxResponse result)
		{
			if (!result.Success)
				return BadRequest(result.Message);

			var taxResource = _mapper.Map<Tax, TaxResource>(result.Tax);
			return Ok(taxResource);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var result = await _taxService.DeleteAsync(id);

			return ReturnResult(result);
		}
	}
}
