using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Dtos
{
	/// <summary>
	/// The movie dto class.
	/// </summary>
	public sealed class MovieDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public int? ID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[StringLength(255)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the release date.
		/// </summary>
		/// <value>
		/// The release date.
		/// </value>
		public DateTime ReleaseDate { get; set; }

		/// <summary>
		/// Gets or sets the genre identifier.
		/// </summary>
		/// <value>
		/// The genre identifier.
		/// </value>
		public int GenreID { get; set; }

		/// <summary>
		/// Gets or sets the genre.
		/// </summary>
		/// <value>
		/// The genre.
		/// </value>
		public GenreDto Genre { get; set; }

		/// <summary>
		/// Gets or sets the date added.
		/// </summary>
		/// <value>
		/// The date added.
		/// </value>
		public DateTime DateAdded { get; set; }

		/// <summary>
		/// Gets or sets the number in stock.
		/// </summary>
		/// <value>
		/// The number in stock.
		/// </value>
		public int NumberInStock { get; set; }
	}
}