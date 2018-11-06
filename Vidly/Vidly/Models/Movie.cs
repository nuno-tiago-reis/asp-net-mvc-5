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
		[StringLength(255)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the release date.
		/// </summary>
		/// <value>
		/// The release date.
		/// </value>
		[Required]
		[Display(Name = "Release Date")]
		public DateTime ReleaseDate { get; set; }

		/// <summary>
		/// Gets or sets the genre identifier.
		/// </summary>
		/// <value>
		/// The genre identifier.
		/// </value>
		[Required]
		[Display(Name = "Genre")]
		public int GenreID { get; set; }

		/// <summary>
		/// Gets or sets the genre.
		/// </summary>
		/// <value>
		/// The genre.
		/// </value>
		public Genre Genre { get; set; }

		/// <summary>
		/// Gets or sets the date added.
		/// </summary>
		/// <value>
		/// The date added.
		/// </value>
		[Required]
		[Display(Name = "Date Added")]
		public DateTime DateAdded { get; set; }

		/// <summary>
		/// Gets or sets the number in stock.
		/// </summary>
		/// <value>
		/// The number in stock.
		/// </value>
		[Required]
		[Display(Name = "Number in Stock")]
		public int NumberInStock { get; set; }
	}
}