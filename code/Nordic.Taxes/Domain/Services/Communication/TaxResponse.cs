using Nordic.Taxes.Domain.Models;

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
