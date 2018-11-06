using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Dtos
{
	/// <summary>
	/// The customer dto class.
	/// </summary>
	public sealed class CustomerDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public int? ID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[StringLength(255)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the birth date.
		/// </summary>
		/// <value>
		/// The birth date.
		/// </value>
		public DateTime? BirthDate { get; set; }

		/// <summary>
		/// Gets or sets the membership type identifier.
		/// </summary>
		/// <value>
		/// The membership type identifier.
		/// </value>
		public int MembershipTypeID { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this customer is subscribed to newsletter.
		/// </summary>
		/// <value>
		///   <c>true</c> if this customer is subscribed to newsletter; otherwise, <c>false</c>.
		/// </value>
		public bool IsSubscribedToNewsletter { get; set; }
	}
}