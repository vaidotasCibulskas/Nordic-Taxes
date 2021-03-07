using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nordic.Taxes.Domain.Models
{
	public class Municipality
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public IList<Tax> Taxes { get; set; } = new List<Tax>();
	}
}

