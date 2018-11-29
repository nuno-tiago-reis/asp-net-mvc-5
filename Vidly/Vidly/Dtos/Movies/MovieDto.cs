using System;
using System.Diagnostics.CodeAnalysis;

namespace Vidly.Dtos
{
	/// <summary>
	/// The movie dto class.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public sealed class MovieDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public int? ID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the release date.
		/// </summary>
		public DateTime ReleaseDate { get; set; }

		/// <summary>
		/// Gets or sets the genre identifier.
		/// </summary>
		public int GenreID { get; set; }

		/// <summary>
		/// Gets or sets the genre.
		/// </summary>
		public GenreDto Genre { get; set; }

		/// <summary>
		/// Gets or sets the date added.
		/// </summary>
		public DateTime DateAdded { get; set; }

		/// <summary>
		/// Gets or sets the number rented.
		/// </summary>
		public int NumberRented { get; set; }

		/// <summary>
		/// Gets or sets the number in stock.
		/// </summary>
		public int NumberInStock { get; set; }
	}
}