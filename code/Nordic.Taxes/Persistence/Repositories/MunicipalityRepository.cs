using Microsoft.EntityFrameworkCore;
using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Domain.Repositories;
using Nordic.Taxes.Persistence.Contexts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nordic.Taxes.Persistence.Repositories
{
	public class MunicipalityRepository : BaseRepository, IMunicipalityRepository
	{
		public MunicipalityRepository(AppDbContext context) : base(context)
		{
		}
		public async Task<IEnumerable<Municipality>> ListAsync()
		{
			return await _context.Municipalities.ToListAsync();
		}
		public async Task AddAsync(Municipality municipality)
		{
			await _context.Municipalities.AddAsync(municipality);
		}

		public async Task<Municipality> FindByIdAsync(int id)
		{
			return await _context.Municipalities.FindAsync(id);
		}

		public void Update(Municipality municipality)
		{
			_context.Municipalities.Update(municipality);
		}

		public void Remove(Municipality municipality)
		{
			_context.Municipalities.Remove(municipality);
		}
	}
}
