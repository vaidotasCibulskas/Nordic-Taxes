using Newtonsoft.Json;
using Nordic.Taxes.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

