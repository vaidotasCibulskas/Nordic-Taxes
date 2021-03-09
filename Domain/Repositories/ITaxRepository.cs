using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nordic.Taxes.Domain.Repositories
{
	public interface ITaxRepository
	{
		Task<IEnumerable<Tax>> ListAsync();
		Task<TaxResponse> AddAsync(Tax tax);
		Task<Tax> GetMunicipalityTaxOfDay(int municipId, DateTime day);
		Task<Tax> FindByIdAsync(int id);
		void Remove(Tax existingTax);
	}
}
