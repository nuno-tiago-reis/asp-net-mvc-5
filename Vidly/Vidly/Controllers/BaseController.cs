using System.Web.Mvc;

namespace Vidly.Controllers
{
	public class BaseController : Controller
	{
		#region [Constants]
		/// <summary>
		/// The message key.
		/// </summary>
		public const string MessageKey = "vidly-message";

		/// <summary>
		/// The message type key.
		/// </summary>
		public const string MessageTypeKey = "vidly-message-type-key";

		/// <summary>
		/// The message type key.
		/// </summary>
		public const string MessageTypeSuccess = "vidly-message-type-success";

		/// <summary>
		/// The message type key.
		/// </summary>
		public const string MessageTypeWarning = "vidly-message-type-warning";

		/// <summary>
		/// The message type key.
		/// </summary>
		public const string MessageTypeError = "vidly-message-type-error";
		#endregion

		#region [TempData]
		/// <inheritdoc />
		protected override ITempDataProvider CreateTempDataProvider()
		{
			return new CookieTempDataProvider();
		}
		#endregion
	}
}