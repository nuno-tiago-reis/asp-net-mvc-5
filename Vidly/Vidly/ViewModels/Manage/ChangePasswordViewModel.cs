using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
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
}