using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Vidly.Models;

namespace Vidly.ViewModels
{
	public sealed class UserFormViewModel
	{
		[Required]
		public ApplicationUser User { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Compare(nameof(Password))]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		public string ConfirmedPassword { get; set; }

		[Required]
		public IEnumerable<string> Roles { get; set; }
	}
}