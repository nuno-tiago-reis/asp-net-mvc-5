using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

using Vidly.Models;

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