using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Vidly.Models
{
	/// <summary>
	/// The genre class.
	/// </summary>
	/// 
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public sealed class Genre
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
	}
}