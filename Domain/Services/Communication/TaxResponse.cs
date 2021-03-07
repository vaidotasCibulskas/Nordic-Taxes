using Nordic.Taxes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nordic.Taxes.Domain.Services.Communication
{
	public class TaxResponse : BaseResponse
	{
        public Tax Tax { get; private set; }

        private TaxResponse(bool success, string message, Tax tax) : base(success, message)
        {
            Tax = tax;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="Tax">Saved Tax.</param>
        /// <returns>Response.</returns>
        public TaxResponse(Tax tax) : this(true, string.Empty, tax)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public TaxResponse(string message) : this(false, message, null)
        { }
    }
}
