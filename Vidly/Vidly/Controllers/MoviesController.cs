using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using Vidly.Identity;
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

		/// <summary>
		/// The user manager field.
		/// </summary>
		private ApplicationUserManager userManagerField;

		/// <summary>
		/// Gets the user manager.
		/// </summary>
		public ApplicationUserManager UserManager
		{
			get
			{
				return this.userManagerField ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				this.userManagerField = value;
			}
		}
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="MoviesController" /> class.
		/// </summary>
		public MoviesController()
		{
			this.context = new ApplicationDbContext();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoviesController" /> class.
		/// </summary>
		/// 
		/// <param name="userManager">The user manager.</param>
		public MoviesController(ApplicationUserManager userManager) : this()
		{
			this.UserManager = userManager;
		}
		#endregion

		/// <summary>
		/// GET: movies/
		/// </summary>
		[HttpGet]
		[Route("movies")]
		public async Task<ActionResult> Index()
		{
			return await this.UserManager.IsInRoleAsync(this.HttpContext.User.Identity.GetUserId(), ApplicationRoles.CanManageMovies)
				? this.View("List")
				: this.View("ReadOnlyList");
		}

		/// <summary>
		/// GET: movies/details/id
		/// </summary>
		/// <param name="id">The movie id.</param>
		[HttpGet]
		[Route("movies/{id}")]
		public async Task<ActionResult> Details(int id)
		{
			var movie = await this.context.Movies.Include(nameof(Movie.Genre)).FirstOrDefaultAsync(m => m.ID == id);
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
		public async Task<ActionResult> Create()
		{
			var viewModel = new MovieFormViewModel
			{
				Genres = await this.context.Genres.ToListAsync()
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
		public async Task<ActionResult> Edit(int id)
		{
			var movie = await this.context.Movies.FirstOrDefaultAsync(c => c.ID == id);
			if (movie == null)
				return this.HttpNotFound();

			var viewModel = new MovieFormViewModel
			{
				Movie = movie,
				Genres = await this.context.Genres.ToListAsync()
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
		public async Task<ActionResult> Delete(int id)
		{
			var movie = await this.context.Movies.FirstOrDefaultAsync(m => m.ID == id);
			if (movie == null)
				return this.HttpNotFound();

			this.context.Movies.Remove(movie);
			await this.context.SaveChangesAsync();

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
		public async Task<ActionResult> Save(Movie movie)
		{
			if (ModelState.IsValid == false)
			{
				var viewModel = new MovieFormViewModel
				{
					Movie = movie,
					Genres = await this.context.Genres.ToListAsync()
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
				var databaseMovie = await this.context.Movies.FirstOrDefaultAsync(c => c.ID == movie.ID);
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

			await this.context.SaveChangesAsync();

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