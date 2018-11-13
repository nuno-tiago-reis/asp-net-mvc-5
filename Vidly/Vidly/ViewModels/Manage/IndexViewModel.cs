using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Identity;

namespace Vidly.Models
{
	/// <summary>
	/// The manage view model.
	/// </summary>
	public sealed class IndexViewModel
	{
		[Required]
		public ApplicationUser User { get; set; }

		[Required]
		public IEnumerable<string> UserRoles { get; set; }

		[Required]
		public IList<UserLoginInfo> UserExternalLogins { get; set; }

		[Required]
		public bool BrowserRemembered { get; set; }
	}
}