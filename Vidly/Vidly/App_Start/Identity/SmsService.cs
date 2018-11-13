using System.Configuration;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.AspNet.Identity;

using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Vidly.Identity
{
	/// <inheritdoc />
	public sealed class SmsService : IIdentityMessageService
	{
		/// <inheritdoc />
		public Task SendAsync(IdentityMessage message)
		{
			return TwilioAsync(message);
		}

		/// <summary>
		/// Sends the message through twilio synchronously.
		/// </summary>
		/// 
		/// <param name="message">The message.</param>
		private static async Task TwilioAsync(IdentityMessage message)
		{
			string id = ConfigurationManager.AppSettings["twilioID"];
			string secret = ConfigurationManager.AppSettings["twilioKey"];
			string fromNumber = ConfigurationManager.AppSettings["twilioFromNumber"];

			TwilioClient.Init(id, secret);
			
			MessageResource.Create
			(
				to : new PhoneNumber(message.Destination),
				from: new PhoneNumber(fromNumber),
				body: message.Body
			);

			await Task.FromResult(0);

			Debug.WriteLine(message.Body);
		}
	}
}