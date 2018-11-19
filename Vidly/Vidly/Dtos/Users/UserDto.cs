using System;
using System.Collections.Generic;

namespace Vidly.Dtos
{
	/// <summary>
	/// The user dto class.
	/// </summary>
	public sealed class UserDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the [email is confirmed].
		/// </summary>
		public bool EmailConfirmed { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the [phone number is confirmed].
		/// </summary>
		public bool PhoneNumberConfirmed { get; set; }

		/// <summary>
		/// Gets or sets the fiscal number.
		/// </summary>
		public string FiscalNumber { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [two factor is enabled].
		/// </summary>
		public bool TwoFactorEnabled { get; set; }

		/// <summary>
		/// Gets or sets the roles.
		/// </summary>
		public IEnumerable<string> Roles { get; set; }
	}
}