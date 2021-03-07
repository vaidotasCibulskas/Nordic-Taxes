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
	public class TaxService : ITaxService
	{
		private readonly ITaxRepository _taxRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<TaxService> _logger;

		public TaxService(ITaxRepository taxyRepository, IUnitOfWork unitOfWork, ILogger<TaxService> logger)
		{
			_taxRepository = taxyRepository;
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		

		public async Task<IEnumerable<Tax>> ListAsync()
		{
			return await _taxRepository.ListAsync();
		}

		public async Task<TaxResponse> SaveAsync(Tax tax)
		{
			try
			{
				await _taxRepository.AddAsync(tax);
				await _unitOfWork.CompleteAsync();

				return new TaxResponse(tax);

			}
			catch (DbUpdateException ex)
			{
				_logger.LogError(ex, $"An error occurred when saving the tax for municipality: {ex.InnerException.Message}");

				if (((SqlException)ex.InnerException).Number == 2601)
					return new TaxResponse($"Tax record ({tax.From} - {tax.To}) for selected municipality already exists.");
				else
					return new TaxResponse($"An error occurred when saving the tax for municipality"); // Todo: use constants or resource for messages
				
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An error occurred when saving the tax for municipality.");
				return new TaxResponse($"An error occurred when saving the tax for municipality");
			}
		}

		public async Task<TaxResponse> GetMunicipalityTaxOfDay(int municipId, DateTime day)
		{
			try
			{
				var tax  = await _taxRepository.GetMunicipalityTaxOfDay(municipId, day);
				if (tax != null)
					return new TaxResponse(tax);
				else
					return new TaxResponse($"For the selected municipality tax records for {day} day do not exist and/or municipality do not exist.");

			}
			catch (Exception ex)
			{
				var errm = $"An error occurred when getting tax for municipality.";
				_logger.LogError(ex, errm);
				return new TaxResponse(errm);
			}
		}

		public async Task<TaxResponse> DeleteAsync(int id)
		{
			var existingTax = await _taxRepository.FindByIdAsync(id);

			if (existingTax == null)
				return new TaxResponse("Tax not found.");

			try
			{
				_taxRepository.Remove(existingTax);
				await _unitOfWork.CompleteAsync();

				return new TaxResponse(existingTax);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An error occurred when deleting the tax: {ex.Message}");
				return new TaxResponse($"An error occurred when deleting the tax");
			}
		}

	}
}
