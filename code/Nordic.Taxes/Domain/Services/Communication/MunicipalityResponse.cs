using Nordic.Taxes.Domain.Models;

namespace Nordic.Taxes.Domain.Services.Communication
{
	public class MunicipalityResponse : BaseResponse
	{
		public Municipality Municipality { get; private set; }

		private MunicipalityResponse(bool success, string message, Municipality municipality) : base(success, message)
		{
			Municipality = municipality;
		}

		/// <summary>
		/// Creates a success response.
		/// </summary>
		/// <param name="municipality">Saved Municipality.</param>
		/// <returns>Response.</returns>
		public MunicipalityResponse(Municipality municipality) : this(true, string.Empty, municipality)
		{ }

		/// <summary>
		/// Creates am error response.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <returns>Response.</returns>
		public MunicipalityResponse(string message) : this(false, message, null)
		{ }
	}
}
