using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Vidly.Models;

namespace Vidly.ViewModels
{
	public sealed class UserFormViewModel
	{
		[Required]
		public ApplicationUser User { get; set; }

		[Required]
		public IEnumerable<string> Roles { get; set; }
	}
}