using System.Collections.Generic;
using Vidly.Models;

namespace Vidly.ViewModels
{
	public sealed class MovieFormViewModel
	{
		/// <summary>
		/// Gets or sets the movie.
		/// </summary>
		/// <value>
		/// The movie.
		/// </value>
		public Movie Movie { get; set; }

		/// <summary>
		/// Gets or sets the genres.
		/// </summary>
		/// <value>
		/// The genres.
		/// </value>
		public IEnumerable<Genre> Genres { get; set; }

	}
}