using AutoMapper;

using System;
using System.Net;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;

using Vidly.Contracts;
using Vidly.Models;

namespace Vidly.Controllers.API
{
	[Authorize]
	public sealed class RentalsController : ApiController
	{
		#region [Properties]
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="RentalsController"/> class.
		/// </summary>
		public RentalsController()
		{
			this.context = new ApplicationDbContext();
		}
		#endregion

		/// <summary>
		/// GET /api/rentals
		/// </summary>
		[HttpGet]
		public IHttpActionResult GetRentals()
		{
			return this.Ok(this.context.Rentals.Include(r => r.Movie).Include(r => r.Customer).AsEnumerable().Select(Mapper.Map<Rental, RentalDto>));
		}

		/// <summary>
		/// GET /api/rentals/id
		/// </summary>
		[HttpGet]
		public IHttpActionResult GetRental(int id)
		{
			var rental = this.context.Rentals.Include(r => r.Movie).Include(r => r.Customer).SingleOrDefault(r => r.ID == id);
			if (rental == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return this.Ok(Mapper.Map<Rental, RentalDto>(rental));
		}

		/// <summary>
		/// POST /api/rentals
		/// </summary>
		[HttpPost]
		[Authorize(Roles = ApplicationRoles.CanManageRentals)]
		public IHttpActionResult CreateRental(RentalDto rentalDto)
		{
			if (this.ModelState.IsValid == false)
				return this.BadRequest();

			var movie = this.context.Movies.FirstOrDefault(m => m.ID == rentalDto.MovieID);
			if(movie == null)
				return this.BadRequest("Movie does not exist.");

			var customer = this.context.Customers.FirstOrDefault(c => c.ID == rentalDto.CustomerID);
			if (customer == null)
				return this.BadRequest("Customer does not exist.");

			if (movie.NumberRented >= movie.NumberInStock)
				return this.BadRequest("Movie is not available.");

			rentalDto.DateRented = DateTime.Now;
			rentalDto.DateReturned = null;

			movie.NumberRented++;

			var rental = Mapper.Map<RentalDto, Rental>(rentalDto);

			this.context.Rentals.Add(rental);
			this.context.SaveChanges();

			rentalDto.ID = rental.ID;

			return this.Created(new Uri($"{Request.RequestUri}/{rental.ID}"), rentalDto);
		}

		/// <summary>
		/// PUT /api/rentals/id
		/// </summary>
		[HttpPut]
		[Authorize(Roles = ApplicationRoles.CanManageRentals)]
		public void UpdateRental(int id, RentalDto rentalDto)
		{
			if (this.ModelState.IsValid == false)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var rental = this.context.Rentals.Include(r => r.Movie).SingleOrDefault(r => r.ID == id);
			if (rental == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			if (rentalDto.DateReturned.HasValue && rental.DateReturned.HasValue == false)
			{
				var databaseMovie = this.context.Movies.First(m => m.ID == rental.Movie.ID);
				databaseMovie.NumberRented--;
			}

			rental.DateReturned = rentalDto.DateReturned ?? rental.DateReturned;

			this.context.SaveChanges();
		}

		/// <summary>
		/// DELETE /api/rentals/id
		/// </summary>
		[HttpDelete]
		[Authorize(Roles = ApplicationRoles.CanManageRentals)]
		public void DeleteRental(int id)
		{
			if (this.ModelState.IsValid == false)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var rental = this.context.Rentals.Include(r => r.Movie).SingleOrDefault(r => r.ID == id);
			if (rental == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			if (rental.DateReturned.HasValue == false)
			{
				var databaseMovie = this.context.Movies.First(m => m.ID == rental.Movie.ID);
				databaseMovie.NumberRented--;
			}

			this.context.Rentals.Remove(rental);
			this.context.SaveChanges();
		}
	}
}