using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Vidly.Models;

namespace Vidly.ViewModels
{
	public sealed class CustomerFormViewModel
	{
		[Required]
		public Customer Customer { get; set; }

		[Required]
		public IEnumerable<MembershipType> MembershipTypes { get; set; }
	}
}