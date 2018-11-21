using System.ComponentModel.DataAnnotations;

namespace Vidly.ViewModels
{
	public sealed class MultipleRentalFormViewModel
	{
		[Required(ErrorMessage = "No customer was selected.")]
		public int? CustomerID { get; set; }

		[Required(ErrorMessage = "No movies were selected.")]
		public int[] MovieIDs { get; set; }
	}
}