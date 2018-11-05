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
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets the sign up fee.
		/// </summary>
		/// <value>
		/// The sign up fee.
		/// </value>
		public short SignUpFee { get; set; }

		/// <summary>
		/// Gets or sets the discount rate.
		/// </summary>
		/// <value>
		/// The discount rate.
		/// </value>
		public byte DiscountRate { get; set; }

		/// <summary>
		/// Gets or sets the duration in months.
		/// </summary>
		/// <value>
		/// The duration in months.
		/// </value>
		public byte DurationInMonths { get; set; }
	}
}