using Nordic.Taxes.Persistence.Contexts;

namespace Nordic.Taxes.Persistence
{
	public abstract class BaseRepository
	{
		protected readonly AppDbContext _context;
		public BaseRepository(AppDbContext context)
		{
			_context = context;
		}
	}
}
