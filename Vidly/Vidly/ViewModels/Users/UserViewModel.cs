using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Vidly.Models;

namespace Vidly.ViewModels
{
	public class UserViewModel
	{
		[Required]
		public ApplicationUser User { get; set; }

		[Required]
		public IEnumerable<string> UserRoles { get; set; }
	}
}