using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Domain.Repositories;
using Nordic.Taxes.Domain.Services;
using Nordic.Taxes.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nordic.Taxes.Services
{
	public class MunicipalityService : IMunicipalityService
	{
		private readonly IMunicipalityRepository _municipalityRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<TaxService> _logger;

		public MunicipalityService(IMunicipalityRepository municipalityRepository, IUnitOfWork unitOfWork, ILogger<TaxService> logger)
		{
			_municipalityRepository = municipalityRepository;
			_unitOfWork = unitOfWork;
			_logger = logger;
		}


		public async Task<IEnumerable<Municipality>> ListAsync()
		{
			return await _municipalityRepository.ListAsync();
		}

		public async Task<MunicipalityResponse> SaveAsync(Municipality municipality)
		{
			try
			{
				await _municipalityRepository.AddAsync(municipality);
				await _unitOfWork.CompleteAsync();

				return new MunicipalityResponse(municipality);
			}
			catch (DbUpdateException ex)
			{
				_logger.LogError(ex, $"An error occurred when saving municipality: {ex.InnerException.Message}");

				if (((SqlException)ex.InnerException).Number == 2601)
					return new MunicipalityResponse($"Municipality with name {municipality.Name} already exists.");
				else
					return new MunicipalityResponse($"An error occurred when saving municipality."); // Todo: use constants or resource for messages

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An error occurred when saving municipality: {ex.InnerException.Message}");
				return new MunicipalityResponse($"An error occurred when saving the municipality");
			}
		}

		public async Task<MunicipalityResponse> UpdateAsync(int id, Municipality municipality)
		{
			var existingMunicipality = await _municipalityRepository.FindByIdAsync(id);

			if (existingMunicipality == null)
				return new MunicipalityResponse("Municipality not found.");

			existingMunicipality.Name = municipality.Name;

			try
			{
				_municipalityRepository.Update(existingMunicipality);
				await _unitOfWork.CompleteAsync();

				return new MunicipalityResponse(existingMunicipality);
			}
			catch (DbUpdateException ex)
			{
				_logger.LogError(ex, $"An error occurred when updating municipality: {ex.InnerException.Message}");

				if (((SqlException)ex.InnerException).Number == 2601)
					return new MunicipalityResponse($"Municipality with name {municipality.Name} already exists.");
				else
					return new MunicipalityResponse($"An error occurred when updating municipality."); // Todo: use constants or resource for messages

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An error occurred when saving municipality: {ex.InnerException.Message}");
				return new MunicipalityResponse($"An error occurred when updating the municipality");
			}
		}

		public async Task<MunicipalityResponse> DeleteAsync(int id)
		{
			var existingMunicipality = await _municipalityRepository.FindByIdAsync(id);

			if (existingMunicipality == null)
				return new MunicipalityResponse("Municipality not found.");

			try
			{
				_municipalityRepository.Remove(existingMunicipality);
				await _unitOfWork.CompleteAsync();

				return new MunicipalityResponse(existingMunicipality);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An error occurred when deleting the municipality: {ex.Message}");
				return new MunicipalityResponse($"An error occurred when deleting the municipality");
			}
		}
	}
}
