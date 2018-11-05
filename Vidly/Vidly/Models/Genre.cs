using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	public sealed class Genre
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
	}
}