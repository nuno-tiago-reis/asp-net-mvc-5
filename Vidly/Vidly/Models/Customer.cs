using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	/// <summary>
	/// The customer class.
	/// </summary>
	public sealed class Customer
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[Required]
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[Required]
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
		[Required]
		public int MembershipTypeID { get; set; }

		/// <summary>
		/// Gets or sets the type of the membership.
		/// </summary>
		/// <value>
		/// The type of the membership.
		/// </value>
		[Required]
		public MembershipType MembershipType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this customer is subscribed to newsletter.
		/// </summary>
		/// <value>
		///   <c>true</c> if this customer is subscribed to newsletter; otherwise, <c>false</c>.
		/// </value>
		[Required]
		public bool IsSubscribedToNewsletter { get; set; }
	}
}