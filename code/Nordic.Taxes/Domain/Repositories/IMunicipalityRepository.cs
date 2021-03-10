using Nordic.Taxes.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nordic.Taxes.Domain.Repositories
{
	public interface IMunicipalityRepository
	{
		Task<IEnumerable<Municipality>> ListAsync();
		Task AddAsync(Municipality municipality);
		Task<Municipality> FindByIdAsync(int id);
		void Update(Municipality municipality);
		void Remove(Municipality municipality);
	}
}
