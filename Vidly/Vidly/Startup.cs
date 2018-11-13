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
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

			// Create the roles
			var roleNames = new[]
			{
				ApplicationRoles.CanManageUsers,
				ApplicationRoles.CanManageMovies,
				ApplicationRoles.CanManageCustomers
			};

			foreach (string roleName in roleNames)
			{
				if (roleManager.RoleExists(roleName))
					continue;

				var role = new IdentityRole { Name = roleName };
				roleManager.Create(role);
			}

			// Create the admin
			if (userManager.Users.Any(user => user.UserName == ApplicationUser.AdminUserName))
				return;

			// Create the user
			var admin = new ApplicationUser
			{
				UserName = ApplicationUser.AdminUserName,

				Email = ApplicationUser.AdminEmail,
				EmailConfirmed = true,

				PhoneNumber = ApplicationUser.AdminPhoneNumber,
				PhoneNumberConfirmed =  true
			};

			var result = userManager.Create(admin, ApplicationUser.AdminPassword);

			// Add the roles to the user
			if (result.Succeeded)
			{
				userManager.AddToRole(admin.Id, ApplicationRoles.CanManageUsers);
				userManager.AddToRole(admin.Id, ApplicationRoles.CanManageMovies);
				userManager.AddToRole(admin.Id, ApplicationRoles.CanManageCustomers);
			}
		}
	}
}