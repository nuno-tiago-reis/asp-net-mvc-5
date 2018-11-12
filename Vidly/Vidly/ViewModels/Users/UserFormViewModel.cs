using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vidly.ViewModels
{
	public sealed class UserFormViewModel : UserViewModel
	{
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Compare(nameof(Password))]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		public string ConfirmedPassword { get; set; }

		[Required]
		public IEnumerable<string> AvailableRoles { get; set; }
	}
}