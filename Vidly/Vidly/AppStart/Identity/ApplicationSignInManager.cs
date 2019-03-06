using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

using Vidly.Models;

namespace Vidly.Identity
{
	/// <summary>
	/// Configure the application sign-in manager which is used in this application.
	/// SignInManager is defined in ASP.NET Identity and is used by the application.
	/// </summary>
	public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationSignInManager"/> class.
		/// </summary>
		/// 
		/// <param name="userManager">The user manager.</param>
		/// <param name="authenticationManager">The authentication manager.</param>
		public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
			: base(userManager, authenticationManager)
		{
		}

		/// <inheritdoc />
		public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
		{
			return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
		}

		/// <summary>
		/// Creates the sign in manager.
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		/// <param name="context">The context.</param>
		public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
		{
			return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
		}
	}
}