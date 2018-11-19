using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	public sealed class ReturnedDateValidationAttribute : ValidationAttribute
	{
		/// <inheritdoc /> 
		/// <summary>
		/// Returns true if the returned date is posterior to the rented date.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var rental = (Rental)context.ObjectInstance;

			return rental.DateReturned < rental.DateRented
				? new ValidationResult("The movie can't be returned before it is rented.") : ValidationResult.Success;
		}
	}
}