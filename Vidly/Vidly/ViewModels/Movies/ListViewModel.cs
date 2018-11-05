using System.Collections.Generic;
using Vidly.Models;

namespace Vidly.ViewModels.Movies
{
	public sealed class ListViewModel
	{
		/// <summary>
		/// Gets or sets the movies.
		/// </summary>
		/// <value>
		/// The movies.
		/// </value>
		public List<Movie> Movies { get; set; }
	}
}