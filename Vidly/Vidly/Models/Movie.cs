using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
	/// <summary>
	/// The movie class.
	/// </summary>
	public sealed class Movie
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
		public string Name { get; set; }
	}
}