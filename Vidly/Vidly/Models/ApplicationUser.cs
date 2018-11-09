using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Vidly.Models
{
	public sealed class ApplicationUser : IdentityUser
	{
		#region [Constants]
		/// <summary>
		/// The admin user name.
		/// </summary>
		public const string AdminUserName = "admin";

		/// <summary>
		/// The admin password.
		/// </summary>
		public const string AdminPassword = "Vidly#2018";

		/// <summary>
		/// The admin email.
		/// </summary>
		public const string AdminEmail = "nuno-tiago-reis@outlook.pt";

		/// <summary>
		/// The admin phone number.
		/// </summary>
		public const string AdminPhoneNumber = "+351 934323293";
		#endregion

		#region [Properties]
		[Required]
		[DisplayName("ID")]
		public override string Id { get; set; }

		[Required]
		[DisplayName("User Name")]
		public override string UserName { get; set; }

		[Required]
		[EmailAddress]
		[DisplayName("Email")]
		public override string Email { get; set; }

		[Required]
		[DisplayName("Email Confirmed?")]
		public override bool EmailConfirmed { get; set; }

		[Phone]
		[DisplayName("Phone Number")]
		public override string PhoneNumber { get; set; }

		[Required]
		[DisplayName("Phone Number Confirmed?")]
		public override bool PhoneNumberConfirmed { get; set; }

		[Required]
		[DisplayName("Two Factor Enabled?")]
		public override bool TwoFactorEnabled { get; set; }

		[Required]
		[DisplayName("Lockout Enabled?")]
		public override bool LockoutEnabled { get; set; }

		[DisplayName("Lockout End Date")]
		public override DateTime? LockoutEndDateUtc { get; set; }

		[DisplayName("Roles")]
		public override ICollection<IdentityUserRole> Roles
		{
			get
			{
				return base.Roles;
			}
		}

		[DisplayName("Roles")]
		public ICollection<string> RoleNames { get; set; }
		#endregion

		#region [Methods]
		/// <summary>
		/// Generates the user identity asynchronously.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <returns></returns>
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

			return userIdentity;
		}
		#endregion
	}	
}