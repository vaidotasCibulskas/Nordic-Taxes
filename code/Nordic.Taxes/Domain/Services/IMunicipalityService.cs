using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Domain.Services.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nordic.Taxes.Domain.Services
{
	public interface IMunicipalityService
	{
		Task<IEnumerable<Municipality>> ListAsync();
		Task<MunicipalityResponse> SaveAsync(Municipality municipality);
		Task<MunicipalityResponse> UpdateAsync(int id, Municipality municipality);
		Task<MunicipalityResponse> DeleteAsync(int id);
	}
}
