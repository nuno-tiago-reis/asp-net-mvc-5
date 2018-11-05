using System.Collections.Generic;
using Vidly.Models;

namespace Vidly.ViewModels.Customers
{
	public sealed class ListViewModel
	{
		/// <summary>
		/// Gets or sets the customers.
		/// </summary>
		/// <value>
		/// The customers.
		/// </value>
		public IEnumerable<Customer> Customers { get; set; }
	}
}