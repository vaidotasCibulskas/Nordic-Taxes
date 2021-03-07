using Newtonsoft.Json;
using Nordic.Taxes.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nordic.Taxes.Resources
{
	public class SpecificDateTaxResource
	{
		public double TaxSize { get; set; }
	}
}

