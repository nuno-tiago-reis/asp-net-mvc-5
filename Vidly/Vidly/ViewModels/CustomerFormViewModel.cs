using System.Collections.Generic;
using Vidly.Models;

namespace Vidly.ViewModels
{
	public sealed class CustomerFormViewModel
	{
		/// <summary>
		/// Gets or sets the customer.
		/// </summary>
		/// <value>
		/// The customer.
		/// </value>
		public Customer Customer { get; set; }

		/// <summary>
		/// Gets or sets the membership types.
		/// </summary>
		/// <value>
		/// The membership types.
		/// </value>
		public IEnumerable<MembershipType> MembershipTypes { get; set; }

	}
}