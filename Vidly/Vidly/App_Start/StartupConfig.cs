using System;
using System.Configuration;
using System.IO;
using System.Linq;

using JetBrains.Annotations;

using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

using Owin;

using Vidly.Identity;
using Vidly.Models;

namespace Vidly
{
	[UsedImplicitly]
	public partial class Startup
	{
		/// <summary>
		/// Configures the authentication.
		/// </summary>
		/// <param name="app">The application.</param>
		public void ConfigureAuth(IAppBuilder app)
		{
			app.CreatePerOwinContext(ApplicationDbContext.Create);
			app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
			app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

			// Enable the application to use a cookie to store information for the signed in user
			// and to use a cookie to temporarily store information about a user logging in with a third party login provider
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login"),
				Provider = new CookieAuthenticationProvider
				{
					// Enables the application to validate the security stamp when the user logs in.
					// This is a security feature which is used when you change a password or add an external login to your account.
					OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>
					(
						validateInterval: TimeSpan.FromMinutes(30),
						regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)
					)
				}
			});

			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
			app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
			// Enables the application to remember the second login verification factor such as phone or email.
			app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

			// Facebook credentials
			app.UseFacebookAuthentication
			(
			   appId: ConfigurationManager.AppSettings["facebookAppID"],
			   appSecret: ConfigurationManager.AppSettings["facebookAppSecret"]
			);

			// Google credentials
			app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
			{
				ClientId = ConfigurationManager.AppSettings["googleAppID"],
				ClientSecret = ConfigurationManager.AppSettings["googleAppSecret"]
			});

			// Twitter credentials
			app.UseTwitterAuthentication
			(
				consumerKey: ConfigurationManager.AppSettings["twitterAppID"],
				consumerSecret: ConfigurationManager.AppSettings["twitterAppSecret"]
				
			);

			// Microsoft credentials
			app.UseMicrosoftAccountAuthentication
			(
				clientId: ConfigurationManager.AppSettings["microsoftAppID"],
				clientSecret: ConfigurationManager.AppSettings["microsoftAppSecret"]
			);
		}

		/// <summary>
		/// Configures the users.
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
				ApplicationRoles.CanManageRentals,
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
				PhoneNumberConfirmed = true,

				FiscalNumber = ApplicationUser.AdminFiscalNumber
			};

			// Add the roles to the user
			var result = userManager.Create(admin, ApplicationUser.AdminPassword);
			if (!result.Succeeded)
				return;

			userManager.AddToRole(admin.Id, ApplicationRoles.CanManageUsers);
			userManager.AddToRole(admin.Id, ApplicationRoles.CanManageMovies);
			userManager.AddToRole(admin.Id, ApplicationRoles.CanManageRentals);
			userManager.AddToRole(admin.Id, ApplicationRoles.CanManageCustomers);
		}

		/// <summary>
		/// Configures the database.
		/// </summary>
		private void ConfigureDatabase()
		{
			var dataDirectory = Directory.GetParent((string)AppDomain.CurrentDomain.GetData("DataDirectory"));
			if (dataDirectory == null)
				return;

			AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(dataDirectory.FullName, "Databases"));
		}
	}
}