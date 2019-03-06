using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace Vidly.Identity
{
	/// <inheritdoc />
	public sealed class EmailService : IIdentityMessageService
	{
		/// <inheritdoc />
		public Task SendAsync(IdentityMessage message)
		{
			return SendGridAsync(message);
		}

		/// <summary>
		/// Sends the message through send grid asynchronously.
		/// </summary>
		/// 
		/// <param name="message">The message.</param>
		private static async Task SendGridAsync(IdentityMessage message)
		{
			var client = new SendGridClient(ConfigurationManager.AppSettings["sendGridKey"]);

			var sendGridMessage = new SendGridMessage();
			sendGridMessage.AddTo(message.Destination);
			sendGridMessage.From = new EmailAddress("no-reply@vidly.com", "Vidly");
			sendGridMessage.Subject = message.Subject;
			sendGridMessage.HtmlContent = message.Body;

			await client.SendEmailAsync(sendGridMessage);

			Console.WriteLine(message.Body);
		}
	}
}