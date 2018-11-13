using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Vidly.Models
{
	/// <summary>
	/// The external logins view model.
	/// </summary>
	public sealed class ExternalLoginsViewModel
	{
		[Required]
		public IList<UserLoginInfo> RegisteredLogins { get; set; }

		[Required]
		public IList<AuthenticationDescription> UnregisteredLogins { get; set; }
	}
}