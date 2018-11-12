using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	public sealed class ReleaseDateValidationAttribute : ValidationAttribute
	{
		/// <inheritdoc /> 
		/// <summary>
		/// Returns true if the added date is posterior to the released date.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var movie = (Movie)context.ObjectInstance;

			return movie.DateAdded < movie.ReleaseDate
				? new ValidationResult("The movie can't be added before it is released.") : ValidationResult.Success;
		}
	}
}