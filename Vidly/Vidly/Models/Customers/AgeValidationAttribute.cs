using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	public sealed class AgeValidationAttribute : ValidationAttribute
	{
		/// <inheritdoc /> 
		/// <summary>
		/// Returns true if the customers age is over 18 years old.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var customer = (Customer)context.ObjectInstance;

			if (customer.MembershipTypeID == MembershipType.Unknown)
				return ValidationResult.Success;

			if (customer.MembershipTypeID == MembershipType.PayAsYouGo)
				return ValidationResult.Success;

			if (customer.BirthDate == null)
				return new ValidationResult("Birth Date is required");

			return DateTime.Today.Year - customer.BirthDate.Value.Year < 18
				? new ValidationResult("Customer should be at least 18 years old to be a member.") : ValidationResult.Success;
		}
	}
}