using System.Collections.Generic;
using Vidly.Models;

namespace Vidly.ViewModels
{
	public sealed class RandomMovieViewModel
	{
		/// <summary>
		/// Gets or sets the movie.
		/// </summary>
		/// <value>
		/// The movie.
		/// </value>
		public Movie Movie { get; set; }

		/// <summary>
		/// Gets or sets the customers.
		/// </summary>
		/// <value>
		/// The customers.
		/// </value>
		public List<Customer> Customers { get; set; }
	}
}