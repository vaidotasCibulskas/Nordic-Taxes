using System;
using System.ComponentModel.DataAnnotations;

namespace Nordic.Taxes.Resources
{
	public class TaxResource
	{
		public int Id { get; set; }

		public string TaxType { get; set; }

		[DataType(DataType.Date)]
		public DateTime From { get; set; }

		[DataType(DataType.Date)]
		public DateTime To { get; set; }
		public double TaxSize { get; set; }

		public MunicipalityResource Municipality { get; set; }
	}
}

