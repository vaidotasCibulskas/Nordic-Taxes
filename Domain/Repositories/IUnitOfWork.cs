using System.Threading.Tasks;

namespace Nordic.Taxes.Domain.Repositories
{
	public interface IUnitOfWork
	{
		Task CompleteAsync();
	}
}
