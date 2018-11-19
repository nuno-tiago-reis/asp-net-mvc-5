using System;
using System.Linq;
using System.Web.Mvc;

using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
	[Authorize]
	public sealed class MoviesController : BaseController
	{
		#region [Properties]
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Vidly.Controllers.MoviesController" /> class.
		/// </summary>
		public MoviesController()
		{
			this.context = new ApplicationDbContext();
		}
		#endregion

		/// <summary>
		/// GET: movies/
		/// </summary>
		[HttpGet]
		[Route("movies")]
		public ViewResult Index()
		{
			return this.User.IsInRole(ApplicationRoles.CanManageMovies)
				? this.View("List")
				: this.View("ReadOnlyList");
		}

		/// <summary>
		/// GET: movies/details/id
		/// </summary>
		/// <param name="id">The movie id.</param>
		[HttpGet]
		[Route("movies/{id}")]
		public ActionResult Details(int id)
		{
			var movie = this.context.Movies.Include(nameof(Movie.Genre)).FirstOrDefault(m => m.ID == id);
			if (movie == null)
				return this.HttpNotFound();

			return this.View("Details", movie);
		}

		/// <summary>
		/// GET: movies/create
		/// </summary>
		[HttpGet]
		[Route("movies/create")]
		[Authorize(Roles = ApplicationRoles.CanManageMovies)]
		public ViewResult Create()
		{
			var viewModel = new MovieFormViewModel
			{
				Genres = this.context.Genres
			};

			//if (System.Runtime.Caching.MemoryCache.Default[nameof(this.context.Genres)] == null)
			//	System.Runtime.Caching.MemoryCache.Default[nameof(this.context.Genres)] = this.context.Genres;

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// GET: movies/edit/id
		/// </summary>
		/// <param name="id">The movie id.</param>
		[HttpGet]
		[Route("movies/edit/{id:regex(\\d)}")]
		[Authorize(Roles = ApplicationRoles.CanManageMovies)]
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

			//if (System.Runtime.Caching.MemoryCache.Default[nameof(this.context.Genres)] == null)
			//	System.Runtime.Caching.MemoryCache.Default[nameof(this.context.Genres)] = this.context.Genres;

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// POST: movies/delete/id
		/// </summary>
		/// <param name="id">The movie id.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = ApplicationRoles.CanManageMovies)]
		public ActionResult Delete(int id)
		{
			var movie = this.context.Movies.FirstOrDefault(m => m.ID == id);
			if (movie == null)
				return this.HttpNotFound();

			this.context.Movies.Remove(movie);
			this.context.SaveChanges();

			this.TempData[MessageKey] = "Movie deleted successfully.";
			this.TempData[MessageTypeKey] = MessageTypeSuccess;

			return this.RedirectToAction("Index", "Movies");
		}

		/// <summary>
		/// Saves the specified movie.
		/// </summary>
		/// <param name="movie">The movie.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = ApplicationRoles.CanManageMovies)]
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
				movie.DateAdded = DateTime.Now;

				this.context.Movies.Add(movie);

				this.TempData[MessageKey] = "Movie created successfully.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}
			else
			{
				var databaseMovie = this.context.Movies.FirstOrDefault(c => c.ID == movie.ID);
				if (databaseMovie == null)
					return this.HttpNotFound();

				this.TryUpdateModel(databaseMovie, nameof(Movie), new[]
				{
					nameof(Movie.Name),
					nameof(Movie.GenreID),
					nameof(Movie.ReleaseDate),
					nameof(Movie.NumberInStock),
					nameof(Movie.DateAdded)
				});

				this.TempData[MessageKey] = "Movie updated successfully.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}

			this.context.SaveChanges();

			return this.RedirectToAction("Index", "Movies");
		}

		#region [CleanUp]
		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			this.context.Dispose();
		}
		#endregion
	}
}