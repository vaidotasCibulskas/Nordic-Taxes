using System;
using System.ComponentModel.DataAnnotations;

namespace Nordic.Taxes.Resources
{
	public class SaveWeeklyTaxResource : BaseSaveTaxResource
	{
		[Required]
		[Range(1, 53,
			ErrorMessage = "Value for {0} must be between {1} and {2}.")]
		public short WeekNumber { get; set; }

	}
}
