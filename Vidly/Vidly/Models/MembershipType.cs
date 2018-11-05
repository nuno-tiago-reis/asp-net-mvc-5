using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	public class MembershipType
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
		/// Gets or sets the sign up fee.
		/// </summary>
		/// <value>
		/// The sign up fee.
		/// </value>
		[Required]
		public short SignUpFee { get; set; }

		/// <summary>
		/// Gets or sets the discount rate.
		/// </summary>
		/// <value>
		/// The discount rate.
		/// </value>
		[Required]
		public byte DiscountRate { get; set; }

		/// <summary>
		/// Gets or sets the duration in months.
		/// </summary>
		/// <value>
		/// The duration in months.
		/// </value>
		[Required]
		public byte DurationInMonths { get; set; }
	}
}