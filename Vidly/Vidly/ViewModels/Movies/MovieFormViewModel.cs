using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Vidly.Models;

namespace Vidly.ViewModels
{
	public sealed class MovieFormViewModel
	{
		[Required]
		public Movie Movie { get; set; }

		[Required]
		public IEnumerable<Genre> Genres { get; set; }
	}
}