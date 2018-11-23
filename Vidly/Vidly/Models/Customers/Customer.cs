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
		[Required]
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the birth date.
		/// </summary>
		[Required]
		[AgeValidation]
		[DataType(DataType.Date)]
		[Display(Name ="Date of Birth")]
		public DateTime? BirthDate { get; set; }

		/// <summary>
		/// Gets or sets the membership type identifier.
		/// </summary>
		[Required]
		[Display(Name = "Membership Type")]
		public int MembershipTypeID { get; set; }

		/// <summary>
		/// Gets or sets the type of the membership.
		/// </summary>
		public MembershipType MembershipType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this customer is subscribed to newsletter.
		/// </summary>
		[Required]
		public bool IsSubscribedToNewsletter { get; set; }
	}
}