using System;
using System.Diagnostics.CodeAnalysis;

namespace Vidly.Dtos
{
	/// <summary>
	/// The customer dto class.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public sealed class CustomerDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public int? ID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the birth date.
		/// </summary>
		public DateTime? BirthDate { get; set; }

		/// <summary>
		/// Gets or sets the membership type identifier.
		/// </summary>
		public int MembershipTypeID { get; set; }

		/// <summary>
		/// Gets or sets the type of the membership.
		/// </summary>
		public MembershipTypeDto MembershipType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this customer is subscribed to newsletter.
		/// </summary>
		public bool IsSubscribedToNewsletter { get; set; }
	}
}