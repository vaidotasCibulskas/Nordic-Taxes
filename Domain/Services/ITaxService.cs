using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nordic.Taxes.Domain.Services
{
	public interface ITaxService
	{
		Task<IEnumerable<Tax>> ListAsync();

		Task<TaxResponse> SaveAsync(Tax tax);

		Task<TaxResponse> GetMunicipalityTaxOfDay(int municipId, DateTime day);

		Task<TaxResponse> DeleteAsync(int id);
	}
}
