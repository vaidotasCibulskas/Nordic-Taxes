using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nordic.Taxes.Domain.Models
{
	public class Tax
	{
		public int Id { get; set; }

		public TaxPeriod TaxType { get; set; }

		[Column(TypeName = "Date")]
		public DateTime From { get; set; }

		[Column(TypeName = "Date")]
		public DateTime To { get; set; }

		[Range(0.0, 1,
			ErrorMessage = "Value for {0} must be between {1} and {2}.")]
		public double TaxSize { get; set; }

		public int MunicipalityId { get; set; }
		public Municipality Municipality { get; set; }
	}
}


