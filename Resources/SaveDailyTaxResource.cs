using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Nordic.Taxes.Domain.Models;

namespace Nordic.Taxes.Resources
{
	public class SaveDailyTaxResource
	{
		[Required]
		[DataType(DataType.Date)]
		public DateTime Day { get; set; }

		[Required]
		[Range(0.0, 1,
			ErrorMessage = "Value for {0} must be between {1} and {2}.")]
		public double TaxSize { get; set; }

		[Required]
		public int MunicipalityId { get; set; }


	}
}
