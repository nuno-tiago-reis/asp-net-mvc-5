using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
	public sealed class MoviesController : Controller
	{
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;

		/// <inheritdoc />
		/// 
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Vidly.Controllers.MoviesController" /> class.
		/// </summary>
		public MoviesController()
		{
			this.context = new ApplicationDbContext();
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			this.context.Dispose();
		}

		/// <summary>
		/// GET: Movies/Index
		/// </summary>
		[Route("movies")]
		public ViewResult Index()
		{
			var movies = this.context.Movies.Include(m => m.Genre);

			return this.View(movies);
		}

		/// <summary>
		/// GET: Movies/Details
		/// </summary>
		[Route("movies/details/{id:regex(\\d)}")]
		public ActionResult Details(int id)
		{
			var movie = this.context.Movies.Include(m => m.Genre).FirstOrDefault(c => c.ID == id);

			if (movie == null)
				return this.HttpNotFound();

			return this.View(movie);
		}

		/// <summary>
		/// GET: Movies/Create
		/// </summary>
		[HttpGet]
		[Route("movies/create")]
		public ViewResult Create()
		{
			var viewModel = new MovieFormViewModel
			{
				Genres = this.context.Genres
			};

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// GET: Movies/Edit
		/// </summary>
		[HttpGet]
		[Route("movies/edit/{id:regex(\\d)}")]
		public ActionResult Edit(int id)
		{
			var movie = this.context.Movies.FirstOrDefault(c => c.ID == id);

			if (movie == null)
				return this.HttpNotFound();

			var viewModel = new MovieFormViewModel
			{
				Movie = movie,
				Genres = this.context.Genres
			};

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// Deletes the movie with the specified ID.
		/// </summary>
		/// <param name="id">The movie id.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			var movie = this.context.Movies.FirstOrDefault(m => m.ID == id);

			if (movie == null)
				return this.HttpNotFound();

			this.context.Movies.Remove(movie);
			this.context.SaveChanges();

			return this.RedirectToAction("Index", "Movies");
		}

		/// <summary>
		/// Saves the specified movie.
		/// </summary>
		/// <param name="movie">The movie.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Save(Movie movie)
		{
			if (ModelState.IsValid == false)
			{
				var viewModel = new MovieFormViewModel
				{
					Movie = movie,
					Genres = this.context.Genres
				};

				return this.View("Form", viewModel);
			}

			if (movie.ID == 0)
			{
				this.context.Movies.Add(movie);
			}
			else
			{
				var databaseMovie = this.context.Movies.FirstOrDefault(c => c.ID == movie.ID);

				this.TryUpdateModel(databaseMovie, nameof(Movie), new[]
				{
					nameof(Movie.Name),
					nameof(Movie.GenreID),
					nameof(Movie.ReleaseDate),
					nameof(Movie.NumberInStock),
					nameof(Movie.DateAdded)
				});
			}

			this.context.SaveChanges();

			return this.RedirectToAction("Index", "Movies");
		}

		/// <summary>
		/// GET: Movies/ByReleaseDate
		/// </summary>
		/// 
		/// <param name="year">The year.</param>
		/// <param name="month">The month.</param>
		public ActionResult ByReleaseDate(int year, int month)
		{
			return this.Content($"Year={year} Month={month}");
		}

	}
}