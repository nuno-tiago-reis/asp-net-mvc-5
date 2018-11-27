using System;
using System.Security.Claims;
using System.Threading.Tasks;
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
		public const string AdminPhoneNumber = "+351934323293";

		/// <summary>
		/// The admin fiscal number.
		/// </summary>
		public const string AdminFiscalNumber = "252704550";
		#endregion

		#region [Properties]
		[Required]
		[DisplayName("ID")]
		public override string Id { get; set; }

		[Required]
		[StringLength(255)]
		[DisplayName("User Name")]
		public override string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Email")]
		public override string Email { get; set; }

		[Required]
		[DisplayName("Email Confirmed?")]
		public override bool EmailConfirmed { get; set; }

		[Required]
		[DataType(DataType.PhoneNumber)]
		[DisplayName("Phone Number")]
		public override string PhoneNumber { get; set; }

		[Required]
		[DisplayName("Phone Number Confirmed?")]
		public override bool PhoneNumberConfirmed { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[DisplayName("Fiscal Number")]
		public string FiscalNumber { get; set; }

		[Required]
		[DisplayName("Lockout Enabled?")]
		public override bool LockoutEnabled { get; set; }

		[DisplayName("Lockout End Date")]
		public override DateTime? LockoutEndDateUtc { get; set; }
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