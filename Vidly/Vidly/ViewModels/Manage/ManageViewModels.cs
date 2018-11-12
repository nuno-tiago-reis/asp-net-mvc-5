using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

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

	/// <summary>
	/// The change password view model.
	/// </summary>
	public sealed class ChangePasswordViewModel
	{
		[Required]
		public bool HasPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Old Password")]
		public string OldPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "New Password")]
		public string NewPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword))]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }
	}

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