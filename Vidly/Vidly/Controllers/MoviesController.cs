using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using Vidly.ViewModels.Movies;

namespace Vidly.Controllers
{
	public sealed class MoviesController : Controller
	{
		/// <summary>
		/// The movies.
		/// </summary>
		private static readonly List<Movie> Movies = new List<Movie>
		{
			new Movie {ID = 1, Name = "Harry Potter and the Philosopher's Stone"},
			new Movie {ID = 2, Name = "Harry Potter and the Chamber of Secrets"},
			new Movie {ID = 3, Name = "Harry Potter and the Prisoner of Azkaban"},
			new Movie {ID = 4, Name = "Harry Potter and the Goblet of Fire"},
			new Movie {ID = 5, Name = "Harry Potter and the Order of the Phoenix"},
			new Movie {ID = 6, Name = "Harry Potter and the Half Blood Prince"},
			new Movie {ID = 7, Name = "Harry Potter and the Deathly Hallows: Part 1"},
			new Movie {ID = 8, Name = "Harry Potter and the Deathly Hallows: Part 2"},
		};

		/// <summary>
		/// GET: Movies/Index
		/// </summary>
		[Route("movies")]
		public ViewResult List()
		{
			var viewModel = new ListViewModel
			{
				Movies = Movies
			};

			return this.View(viewModel);
		}

		/// <summary>
		/// GET: Movies/Details
		/// </summary>
		[Route("movies/details/{id:regex(\\d)}")]
		public ViewResult Details(int id)
		{
			var movie = Movies.FirstOrDefault(m => m.ID == id);

			return this.View(movie);
		}

		/// <summary>
		/// GET: Movies/Random
		/// </summary>
		public ViewResult Random()
		{
			var movie = new Movie
			{
				ID = 0,
				Name = "Shrek"
			};

			var customers = new List<Customer>
			{
				new Customer { ID = 0, Name = "Customer 1" },
				new Customer { ID = 0, Name = "Customer 2" }
			};

			var viewModel = new RandomMovieViewModel
			{
				Movie = movie, Customers = customers
			};

			return this.View(viewModel);
		}

		public ActionResult ByReleaseDate(int year, int month)
		{
			return this.Content($"Year={year} Month={month}");
		}

		/// <summary>
		/// GET: Movies/HelloWorld
		/// </summary>
		public ContentResult HelloWorld()
		{
			return this.Content("Hello World!");
		}


		/// <summary>
		/// GET: Movies/Redirect
		/// </summary>
		public RedirectToRouteResult Redirect()
		{
			return this.RedirectToAction("Index", "Home", new { page = 1, sortBy ="name" });
		}
	}
}