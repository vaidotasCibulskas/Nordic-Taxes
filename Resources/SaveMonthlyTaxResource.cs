using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Nordic.Taxes.Domain.Models;

namespace Nordic.Taxes.Resources
{
	public class SaveMonthlyTaxResource: BaseSaveTaxResource
	{
		[Required]
		[Range(1, 12,
			ErrorMessage = "Value for {0} must be between {1} and {2}.")]
		public short Month { get; set; }

	}
}
