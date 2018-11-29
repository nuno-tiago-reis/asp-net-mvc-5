using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Vidly.Models
{
	/// <summary>
	/// The rental class.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public sealed class Rental
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		[Required]
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets the movie identifier.
		/// </summary>
		[Required]
		public int MovieID { get; set; }

		/// <summary>
		/// Gets or sets the movie.
		/// </summary>
		public Movie Movie { get; set; }

		/// <summary>
		/// Gets or sets the customer identifier.
		/// </summary>
		[Required]
		public int CustomerID { get; set; }

		/// <summary>
		/// Gets or sets the customer.
		/// </summary>
		public Customer Customer { get; set; }

		/// <summary>
		/// Gets or sets the date rented.
		/// </summary>
		[Required]
		[Display(Name="Rent Date")]
		public DateTime DateRented { get; set; }

		/// <summary>
		/// Gets or sets the date returned.
		/// </summary>
		[ReturnedDateValidation]
		[Display(Name = "Return Date")]
		public DateTime? DateReturned { get; set; }
	}
}