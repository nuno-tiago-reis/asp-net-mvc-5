using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Owin;

using Vidly.Models;

[assembly: OwinStartup(typeof(Vidly.Startup))]
namespace Vidly
{
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public partial class Startup
	{
		/// <summary>
		/// Configurations the specified application.
		/// </summary>
		/// <param name="app">The application.</param>
		public void Configuration(IAppBuilder app)
		{
			this.ConfigureAuth(app);
			this.ConfigureUsers();
		}

		/// <summary>
		/// Configures the default users, admin and guest.
		/// </summary>
		private void ConfigureUsers()
		{
			var context = new ApplicationDbContext();

			// Roles
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

			if (!roleManager.RoleExists(ApplicationUser.CanManageUsersRole))
			{
				var role = new IdentityRole { Name = ApplicationUser.CanManageUsersRole };
				roleManager.Create(role);
			}
			
			if (!roleManager.RoleExists(ApplicationUser.CanManageMoviesRole))
			{
				var role = new IdentityRole { Name = ApplicationUser.CanManageMoviesRole };
				roleManager.Create(role);
			}
			
			if (!roleManager.RoleExists(ApplicationUser.CanManageCustomersRole))
			{
				var role = new IdentityRole { Name = ApplicationUser.CanManageCustomersRole };
				roleManager.Create(role);
			}

			// Admin
			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

			if (!userManager.Users.Any(user => user.UserName == ApplicationUser.AdminUserName))
			{
				// Create the user
				var user = new ApplicationUser
				{
					UserName = ApplicationUser.AdminUserName,

					Email = ApplicationUser.AdminEmail,
					EmailConfirmed = true,

					PhoneNumber = ApplicationUser.AdminPhoneNumber,
					PhoneNumberConfirmed =  true
				};

				var result = userManager.Create(user, ApplicationUser.AdminPassword);

				// Add the roles to the user
				if (result.Succeeded)
				{
					userManager.AddToRole(user.Id, ApplicationUser.CanManageUsersRole);
					userManager.AddToRole(user.Id, ApplicationUser.CanManageMoviesRole);
					userManager.AddToRole(user.Id, ApplicationUser.CanManageCustomersRole);
				}
			}
		}
	}
}
