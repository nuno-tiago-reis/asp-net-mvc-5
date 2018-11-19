﻿using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	public sealed class MembershipType
	{
		/// <summary>
		/// The unknown membership type id.
		/// </summary>
		public static readonly byte Unknown = 0;

		/// <summary>
		/// The pay as you go membership type id.
		/// </summary>
		public static readonly byte PayAsYouGo = 1;

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
		/// Gets or sets the sign up fee.
		/// </summary>
		[Required]
		public short SignUpFee { get; set; }

		/// <summary>
		/// Gets or sets the discount rate.
		/// </summary>
		[Required]
		public byte DiscountRate { get; set; }

		/// <summary>
		/// Gets or sets the duration in months.
		/// </summary>
		[Required]
		public byte DurationInMonths { get; set; }
	}
}