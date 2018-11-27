using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	/// <summary>
	/// The change phone number view model.
	/// </summary>
	public sealed class ChangePhoneNumberViewModel
	{
		[Required]
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }
	}
}