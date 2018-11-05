using System;
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

		/// <summary>
		/// Gets or sets the number in stock.
		/// </summary>
		/// <value>
		/// The number in stock.
		/// </value>
		[Required]
		public int NumberInStock { get; set; }

		/// <summary>
		/// Gets or sets the date added.
		/// </summary>
		/// <value>
		/// The date added.
		/// </value>
		[Required]
		public DateTime DateAdded { get; set; }

		/// <summary>
		/// Gets or sets the release date.
		/// </summary>
		/// <value>
		/// The release date.
		/// </value>
		[Required]
		public DateTime ReleaseDate { get; set; }

		/// <summary>
		/// Gets or sets the genre identifier.
		/// </summary>
		/// <value>
		/// The genre identifier.
		/// </value>
		[Required]
		public int GenreID { get; set; }

		/// <summary>
		/// Gets or sets the genre.
		/// </summary>
		/// <value>
		/// The genre.
		/// </value>
		[Required]
		public Genre Genre { get; set; }
	}
}