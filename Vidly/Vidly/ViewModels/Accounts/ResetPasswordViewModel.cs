using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	/// <summary>
	/// The reset password view model.
	/// </summary>
	public sealed class ResetPasswordViewModel
	{
		[Required]
		[DataType(DataType.Text)]
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
}