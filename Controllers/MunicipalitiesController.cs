﻿using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Domain.Services;
using Nordic.Taxes.Extensions;
using Nordic.Taxes.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nordic.Taxes.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class MunicipalitiesController : ControllerBase
	{

		private readonly ILogger<MunicipalitiesController> _logger;
		private readonly IMunicipalityService _municipalityService;
		private readonly IMapper _mapper;


		public MunicipalitiesController(ILogger<MunicipalitiesController> logger, IMunicipalityService municipalityService, IMapper mapper)
		{
			_logger = logger;
			_municipalityService = municipalityService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IEnumerable<MunicipalityResource>> GetAllAsync()
		{
			var municipalities = await _municipalityService.ListAsync();
			var resources = _mapper.Map<IEnumerable<Municipality>, IEnumerable<MunicipalityResource>>(municipalities);

			return resources;
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] SaveMunicipalityResource resource)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState.GetErrorMessages());
			var municipality = _mapper.Map<SaveMunicipalityResource, Municipality>(resource);
			var result = await _municipalityService.SaveAsync(municipality);

			if (!result.Success)
				return BadRequest(result.Message);

			var municipalityResource = _mapper.Map<Municipality, MunicipalityResource>(result.Municipality);
			return Ok(municipalityResource);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutAsync(int id, [FromBody] SaveMunicipalityResource resource)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState.GetErrorMessages());

			var municipality = _mapper.Map<SaveMunicipalityResource, Municipality>(resource);
			var result = await _municipalityService.UpdateAsync(id, municipality);

			if (!result.Success)
				return BadRequest(result.Message);

			var municipalityResource = _mapper.Map<Municipality, MunicipalityResource>(result.Municipality);
			return Ok(municipalityResource);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var result = await _municipalityService.DeleteAsync(id);

			if (!result.Success)
				return BadRequest(result.Message);

			var municipalityResource = _mapper.Map<Municipality, MunicipalityResource>(result.Municipality);
			return Ok(municipalityResource);
		}

		[HttpPost("import")]
		[HttpPost, DisableRequestSizeLimit]
		public async Task<IActionResult> Upload()
		{
			try
			{
				var formCollection = await Request.ReadFormAsync();
				var file = formCollection.Files[0];

				if (file.Length > 0)
				{
					var results = new List<object>();
					using (var stream = new MemoryStream())
					{
						file.CopyTo(stream);
						stream.Seek(0, SeekOrigin.Begin);
						using (TextReader tr = new StreamReader(stream))
						{
							using (var csvReader = new CsvReader(tr, CultureInfo.InvariantCulture))
							{
								csvReader.Read();
								csvReader.ReadHeader();
								while (csvReader.Read())
								{
									var record = csvReader.GetRecord<SaveMunicipalityResource>();
									var municipality = _mapper.Map<SaveMunicipalityResource, Municipality>(record);
									var result = await _municipalityService.SaveAsync(municipality);
									if (!result.Success)
										results.Add(result.Message);
									else
									{
										var municipalityResource = _mapper.Map<Municipality, MunicipalityResource>(result.Municipality);
										results.Add(municipalityResource);
									}
								}
							}
						}

					}

					return Ok(results);
				}
				else
				{
					return BadRequest("File is not added.");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
	}
}
