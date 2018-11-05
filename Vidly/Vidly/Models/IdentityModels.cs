using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Vidly.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		/// <summary>
		/// Gets or sets the customers.
		/// </summary>
		/// <value>
		/// The customers.
		/// </value>
		public DbSet<Customer> Customers { get; set; }

		/// <summary>
		/// Gets or sets the movies.
		/// </summary>
		/// <value>
		/// The movies.
		/// </value>
		public DbSet<Movie> Movies { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
		/// </summary>
		public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns></returns>
		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}