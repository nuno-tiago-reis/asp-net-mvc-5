using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.ViewModels
{
	public sealed class RentalFormViewModel
	{
		public int? ID { get; set; }

		[Required(ErrorMessage = "No movie was selected.")]
		public int? MovieID { get; set; }

		[Required(ErrorMessage = "No customer was selected.")]
		public int? CustomerID { get; set; }

		[Display(Name = "Date Rented")]
		public DateTime? DateRented { get; set; }

		[Display(Name = "Date Returned")]
		public DateTime? DateReturned { get; set; }
	}
}