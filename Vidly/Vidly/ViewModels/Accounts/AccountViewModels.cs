using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	/// <summary>
	/// The login view model.
	/// </summary>
	public sealed class LoginViewModel
	{
		[Required]
		[Display(Name = "User or Email")]
		public string UserOrEmail { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}

	/// <summary>
	/// The register view model.
	/// </summary>
	public sealed class RegisterViewModel
	{
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "User")]
		public string UserName { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Phone]
		[Display(Name = "Phone Number (Optional)")]
		public string PhoneNumber { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "FiscalNumber")]
		public string FiscalNumber { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	/// <summary>
	/// The reset password view model.
	/// </summary>
	public sealed class ResetPasswordViewModel
	{
		[Required]
		[Display(Name = "User or Email")]
		public string UserOrEmail { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Type password")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required]
		public string Token { get; set; }
	}

	/// <summary>
	/// The forgot password view model.
	/// </summary>
	public sealed class ForgotPasswordViewModel
	{
		[Required]
		[Display(Name = "User or Email")]
		public string UserOrEmail { get; set; }
	}

	/// <summary>
	/// The send two factor authentication code view model. TODO
	/// </summary>
	public class SendCodeViewModel
	{
		[Display(Name = "Provider")]
		public string SelectedProvider { get; set; }

		[Display(Name = "Providers")]
		public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }
	}

	/// <summary>
	/// The verify two factor authentication code view model. TODO
	/// </summary>
	public class VerifyCodeViewModel
	{
		[Required]
		[Display(Name = "Provider")]
		public string Provider { get; set; }

		[Required]
		[Display(Name = "Code")]
		public string Code { get; set; }

		[Required]
		[Display(Name = "Remember this browser?")]
		public bool RememberBrowser { get; set; }

		[Required]
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }
	}

	/// <summary>
	/// The external login list view model.
	/// </summary>
	public sealed class ExternalLoginListViewModel
	{
		public string ReturnUrl { get; set; }
	}

	/// <summary>
	/// The external login confirmation view model.
	/// </summary>
	public sealed class ExternalLoginConfirmationViewModel
	{
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "User Name")]
		public string UserName { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Phone]
		[Display(Name = "Phone Number (Optional)")]
		public string PhoneNumber { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Fiscal Number")]
		public string FiscalNumber { get; set; }
	}
}
