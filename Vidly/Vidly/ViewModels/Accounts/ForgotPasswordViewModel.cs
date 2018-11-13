using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	/// <summary>
	/// The forgot password view model.
	/// </summary>
	public sealed class ForgotPasswordViewModel
	{
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "User or Email")]
		public string UserOrEmail { get; set; }
	}
}