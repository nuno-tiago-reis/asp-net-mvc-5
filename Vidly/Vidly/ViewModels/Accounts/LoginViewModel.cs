using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	/// <summary>
	/// The login view model.
	/// </summary>
	public sealed class LoginViewModel
	{
		[Required]
		[DataType(DataType.Text)]
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
}