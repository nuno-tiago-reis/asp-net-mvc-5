using System.ComponentModel.DataAnnotations;

namespace Vidly.Dtos
{
	public sealed class MembershipTypeDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[StringLength(255)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the discount rate.
		/// </summary>
		public byte DiscountRate { get; set; }
	}
}