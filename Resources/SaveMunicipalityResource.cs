using System.ComponentModel.DataAnnotations;

namespace Nordic.Taxes.Resources
{
	public class SaveMunicipalityResource
	{
		[Required]
		[MaxLength(128)]
		public string Name { get; set; }
	}
}
