using System.Data.Entity;

using Microsoft.AspNet.Identity.EntityFramework;

namespace Vidly.Models
{
	public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the customers.
		/// </summary>
		public DbSet<Customer> Customers { get; set; }

		/// <summary>
		/// Gets or sets the membership types.
		/// </summary>
		public DbSet<MembershipType> MembershipTypes { get; set; }

		/// <summary>
		/// Gets or sets the movies.
		/// </summary>
		public DbSet<Movie> Movies { get; set; }

		/// <summary>
		/// Gets or sets the genres.
		/// </summary>
		public DbSet<Genre> Genres { get; set; }

		/// <summary>
		/// Gets or sets the rentals.
		/// </summary>
		public DbSet<Rental> Rentals { get; set; }
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
		/// </summary>
		public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		/// <summary>
		/// Creates this instance.
		/// </summary>
		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
		#endregion
	}
}