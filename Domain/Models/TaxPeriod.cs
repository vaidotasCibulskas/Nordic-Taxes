using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Nordic.Taxes.Domain.Models
{
	public enum TaxPeriod: byte
	{
        [Description("Yearly tax")]
        Year = 1,

        [Description("Monthly tax")]
        Month = 2,

        [Description("Weekly tax")]
        Week = 3,

        [Description("Daily tax")]
        Day = 4
    }
}
