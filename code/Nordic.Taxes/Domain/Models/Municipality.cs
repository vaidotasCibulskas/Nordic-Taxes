using System.Collections.Generic;

namespace Nordic.Taxes.Domain.Models
{
	public class Municipality
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public IList<Tax> Taxes { get; set; } = new List<Tax>();
	}
}

