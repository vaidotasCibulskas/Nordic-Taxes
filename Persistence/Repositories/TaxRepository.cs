using Microsoft.EntityFrameworkCore;
using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Domain.Repositories;
using Nordic.Taxes.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nordic.Taxes.Persistence.Repositories
{
	public class TaxRepository : BaseRepository, ITaxRepository
	{
		public TaxRepository(AppDbContext context) : base(context)
		{
		}
		public async Task<IEnumerable<Tax>> ListAsync()
		{
			return await _context.Taxes
				.Include(tax => tax.Municipality)
				.ToListAsync();
		}


		public async Task AddAsync(Tax tax)
		{
			await _context.Taxes.AddAsync(tax);
		}

		public Task<Tax> GetMunicipalityTaxOfDay(int municipId, DateTime day)
		{
			return Task.Run
			(() =>
				_context.Taxes.Where(x => 
				x.MunicipalityId == municipId && 
				x.From <= day && 
				x.To >= day).OrderBy(x => x.TaxType).FirstOrDefault()
			);

		}

		public async Task<Tax> FindByIdAsync(int id)
		{
			return await _context.Taxes.FindAsync(id);
		}

		public void Remove(Tax existingTax)
		{
			_context.Taxes.Remove(existingTax);
		}
	}
}
