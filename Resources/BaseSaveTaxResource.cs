using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Nordic.Taxes.Resources
{
	public abstract class BaseSaveTaxResource
	{
		[Required]
		[Range(1900, 2999,
			ErrorMessage = "Value for {0} must be between {1} and {2}.")]
		public int Year { get; set; }

		[Required]
		[Range(0.0, 1,
			ErrorMessage = "Value for {0} must be between {1} and {2}.")]
		public double TaxSize { get; set; }

		[Required]
		public int MunicipalityId { get; set; }
	}
}
