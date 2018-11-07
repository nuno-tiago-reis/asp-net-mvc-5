using System.ComponentModel.DataAnnotations;

namespace Vidly.Dtos
{
	public sealed class MembershipTypeDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[StringLength(255)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the discount rate.
		/// </summary>
		/// <value>
		/// The discount rate.
		/// </value>
		public byte DiscountRate { get; set; }
	}
}