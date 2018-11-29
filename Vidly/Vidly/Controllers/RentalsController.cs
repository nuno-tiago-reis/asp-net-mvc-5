using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
	[Authorize]
	public sealed class RentalsController : BaseController
	{
		#region [Properties]
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="RentalsController" /> class.
		/// </summary>
		public RentalsController()
		{
			this.context = new ApplicationDbContext();
		}
		#endregion

		/// <summary>
		/// GET: rentals/
		/// </summary>
		[HttpGet]
		[Route("rentals")]
		public ViewResult Index()
		{
			return this.User.IsInRole(ApplicationRoles.CanManageRentals)
				? this.View("List")
				: this.View("ReadOnlyList");
		}

		/// <summary>
		/// GET: rentals/details/id
		/// </summary>
		/// <param name="id">The rental id.</param>
		[HttpGet]
		[Route("rentals/{id}")]
		public ActionResult Details(int id)
		{
			var rental = this.context.Rentals.Include(r => r.Movie).Include(r => r.Customer).FirstOrDefault(r => r.ID == id);
			if (rental == null)
				return this.HttpNotFound();

			return this.View("Details", rental);
		}

		/// <summary>
		/// GET: rentals/create
		/// </summary>
		[HttpGet]
		[Route("rentals/create")]
		[Authorize(Roles = ApplicationRoles.CanManageRentals)]
		public ViewResult Create()
		{
			return this.View("Form", new RentalFormViewModel());
		}

		/// <summary>
		/// GET: rentals/createmultiple
		/// </summary>
		[HttpGet]
		[Route("rentals/createmultiple")]
		[Authorize(Roles = ApplicationRoles.CanManageRentals)]
		public ViewResult CreateMultiple()
		{
			return this.View("MultipleForm", new MultipleRentalFormViewModel());
		}

		/// <summary>
		/// GET: rentals/edit/id
		/// </summary>
		/// <param name="id">The rental id.</param>
		[HttpGet]
		[Route("rentals/edit/{id:regex(\\d)}")]
		[Authorize(Roles = ApplicationRoles.CanManageRentals)]
		public ActionResult Edit(int id)
		{
			var rental = this.context.Rentals.Include(r => r.Movie).Include(r => r.Customer).FirstOrDefault(r => r.ID == id);
			if (rental == null)
				return this.HttpNotFound();

			var viewModel = new RentalFormViewModel
			{
				DateReturned = rental.DateReturned
			};

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// POST: rentals/delete/id
		/// </summary>
		/// <param name="id">The rental id.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = ApplicationRoles.CanManageRentals)]
		public ActionResult Delete(int id)
		{
			var rental = this.context.Rentals.Include(r => r.Movie).Include(r => r.Customer).FirstOrDefault(m => m.ID == id);
			if (rental == null)
				return this.HttpNotFound();

			if (rental.DateReturned.HasValue == false)
			{
				var databaseMovie = this.context.Movies.First(m => m.ID == rental.Movie.ID);
				databaseMovie.NumberRented--;
			}

			this.context.Rentals.Remove(rental);
			this.context.SaveChanges();

			this.TempData[MessageKey] = "Rental deleted successfully.";
			this.TempData[MessageTypeKey] = MessageTypeSuccess;

			return this.RedirectToAction("Index", "Rentals");
		}

		/// <summary>
		/// Saves the specified rental.
		/// </summary>
		/// <param name="rental">The rental.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = ApplicationRoles.CanManageRentals)]
		public ActionResult Save(Rental rental)
		{
			if (ModelState.IsValid == false)
			{
				var viewModel = new RentalFormViewModel
				{
					MovieID = rental.Movie?.ID ?? 0,
					CustomerID = rental.Customer?.ID ?? 0,
					DateRented = rental.DateRented,
					DateReturned = rental.DateReturned
				};

				return this.View("Form", viewModel);
			}

			if (rental.ID == 0)
			{
				rental.DateRented = DateTime.Now;

				var databaseMovie = this.context.Movies.First(m => m.ID == rental.MovieID);
				if (databaseMovie.NumberInStock == databaseMovie.NumberRented)
				{
					this.TempData[MessageKey] = $"The movie {databaseMovie.Name} is out of stock.";
					this.TempData[MessageTypeKey] = MessageTypeError;

					return this.RedirectToAction("Index", "Rentals");
				}

				databaseMovie.NumberRented++;

				this.context.Rentals.Add(rental);

				this.TempData[MessageKey] = "Rental created successfully.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}
			else
			{
				var databaseRental = this.context.Rentals.FirstOrDefault(r => r.ID == rental.ID);
				if (databaseRental == null)
					return this.HttpNotFound();

				bool returned = rental.DateReturned.HasValue && databaseRental.DateReturned.HasValue == false;

				bool result = this.TryUpdateModel(databaseRental, nameof(Rental), new[]
				{
					nameof(Rental.Movie),
					nameof(Rental.Customer),
					nameof(Rental.DateRented),
					nameof(Rental.DateReturned)
				});

				if (result && returned)
				{
					var databaseMovie = this.context.Movies.First(m => m.ID == rental.MovieID);
					databaseMovie.NumberRented--;
				}

				this.TempData[MessageKey] = "Rental updated successfully.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}

			this.context.SaveChanges();

			return this.RedirectToAction("Index", "Rentals");
		}

		/// <summary>
		/// Saves the specified rentals.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = ApplicationRoles.CanManageRentals)]
		public ActionResult SaveMultiple(MultipleRentalFormViewModel viewModel)
		{
			if (ModelState.IsValid == false)
			{
				return this.View("MultipleForm", viewModel);
			}

			string warningMessage = string.Empty;

			foreach (int movieID in viewModel.MovieIDs)
			{
				// ReSharper disable once PossibleInvalidOperationException
				var rental = new Rental
				{
					CustomerID = viewModel.CustomerID.Value,
					MovieID = movieID,
					DateRented = DateTime.Now
				};

				var databaseMovie = this.context.Movies.First(m => m.ID == rental.MovieID);
				if (databaseMovie.NumberInStock == databaseMovie.NumberRented)
				{
					warningMessage += $"<br>&bull; The movie {databaseMovie.Name} is out of stock.";
				}
				else
				{
					databaseMovie.NumberRented++;

					this.context.Rentals.Add(rental);
				}
			}

			this.context.SaveChanges();

			if (string.IsNullOrWhiteSpace(warningMessage))
			{
				this.TempData[MessageKey] = "Rentals created successfully.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}
			else
			{
				this.TempData[MessageKey] = "Rentals created successfully with exceptions:" + warningMessage;
				this.TempData[MessageTypeKey] = MessageTypeWarning;
			}

			return this.RedirectToAction("Index", "Rentals");
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