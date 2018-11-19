using System;

using Vidly.Models;

namespace Vidly.Dtos
{
	/// <summary>
	/// The rental dto class.
	/// </summary>
	public sealed class RentalDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public int? ID { get; set; }

		/// <summary>
		/// Gets or sets the movie identifier.
		/// </summary>
		public int MovieID { get; set; }

		/// <summary>
		/// Gets or sets the movie.
		/// </summary>
		public Movie Movie { get; set; }

		/// <summary>
		/// Gets or sets the customer identifier.
		/// </summary>
		public int CustomerID { get; set; }

		/// <summary>
		/// Gets or sets the customer.
		/// </summary>
		public Customer Customer { get; set; }

		/// <summary>
		/// Gets or sets the date rented.
		/// </summary>
		public DateTime? DateRented { get; set; }

		/// <summary>
		/// Gets or sets the date returned.
		/// </summary>
		public DateTime? DateReturned { get; set; }
	}
}