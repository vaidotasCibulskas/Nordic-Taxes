using Nordic.Taxes.Domain.Repositories;
using Nordic.Taxes.Persistence.Contexts;
using System.Threading.Tasks;

namespace Nordic.Taxes.Persistence.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _context;

		public UnitOfWork(AppDbContext context)
		{
			_context = context;
		}

		public async Task CompleteAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
