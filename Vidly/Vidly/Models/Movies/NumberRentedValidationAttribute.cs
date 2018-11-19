using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	public sealed class NumberRentedValidationAttribute : ValidationAttribute
	{
		/// <inheritdoc /> 
		/// <summary>
		/// Returns true if the number of rented is smaller or equal to the number in stock.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var movie = (Movie)context.ObjectInstance;

			return movie.NumberRented > movie.NumberInStock
				? new ValidationResult("The movie can't have more rented units than in stock.") : ValidationResult.Success;
		}
	}
}