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
		[Required]
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the release date.
		/// </summary>
		[Required]
		[ReleaseDateValidation]
		[DataType(DataType.Date)]
		[Display(Name = "Release Date")]
		public DateTime ReleaseDate { get; set; }

		/// <summary>
		/// Gets or sets the genre identifier.
		/// </summary>
		[Required]
		[Display(Name = "Genre")]
		public int GenreID { get; set; }

		/// <summary>
		/// Gets or sets the genre.
		/// </summary>
		public Genre Genre { get; set; }

		/// <summary>
		/// Gets or sets the date added.
		/// </summary>
		[Required]
		[DataType(DataType.Date)]
		[Display(Name = "Date Added")]
		public DateTime DateAdded { get; set; }

		/// <summary>
		/// Gets or sets the number rented.
		/// </summary>
		[Required]
		[NumberRentedValidation]
		[Display(Name = "Number Rented")]
		public int NumberRented { get; set; }

		/// <summary>
		/// Gets or sets the number in stock.
		/// </summary>
		[Required]
		[Display(Name = "Number In Stock")]
		public int NumberInStock { get; set; }
	}
}