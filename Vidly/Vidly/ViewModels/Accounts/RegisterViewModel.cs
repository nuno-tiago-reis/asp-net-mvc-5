using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
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
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Fiscal Number")]
		public string FiscalNumber { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}