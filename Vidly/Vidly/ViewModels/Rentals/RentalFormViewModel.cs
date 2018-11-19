using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.ViewModels
{
	public sealed class RentalFormViewModel
	{
		public int? ID { get; set; }

		[Required]
		public int MovieID { get; set; }

		[Required]
		[Display(Name = "Movie Name")]
		public string MovieName { get; set; }

		[Required]
		public int CustomerID { get; set; }

		[Required]
		[Display(Name = "Customer Name")]
		public string CustomerName { get; set; }

		[Display(Name = "Date Rented")]
		public DateTime? DateRented { get; set; }

		[Display(Name = "Date Returned")]
		public DateTime? DateReturned { get; set; }
	}
}