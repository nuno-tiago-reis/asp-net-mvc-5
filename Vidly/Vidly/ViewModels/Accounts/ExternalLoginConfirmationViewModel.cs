using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
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
	}
}