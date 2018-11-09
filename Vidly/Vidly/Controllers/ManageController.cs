using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Vidly.Models;

namespace Vidly.Controllers
{
	[Authorize]
	public class ManageController : Controller
	{
		#region [Properties]
		/// <summary>
		/// The sign in manager field.
		/// </summary>
		private ApplicationSignInManager signInManagerField;

		/// <summary>
		/// The user manager field.
		/// </summary>
		private ApplicationUserManager userManagerField;

		/// <summary>
		/// Gets the sign in manager.
		/// </summary>
		/// <value>
		/// The sign in manager.
		/// </value>
		public ApplicationSignInManager SignInManager
		{
			get
			{
				return this.signInManagerField ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				this.signInManagerField = value;
			}
		}

		/// <summary>
		/// Gets the user manager.
		/// </summary>
		/// <value>
		/// The user manager.
		/// </value>
		public ApplicationUserManager UserManager
		{
			get
			{
				return this.userManagerField ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				this.userManagerField = value;
			}
		}
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="ManageController"/> class.
		/// </summary>
		public ManageController()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageController"/> class.
		/// </summary>
		/// <param name="userManager">The user manager.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			this.UserManager = userManager;
			this.SignInManager = signInManager;
		}
		#endregion

		/// <summary>
		/// GET: /Manage/Index
		/// </summary>
		public async Task<ActionResult> Index(ManageMessageId? message)
		{
			this.ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
				: message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
				: message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
				: message == ManageMessageId.Error ? "An error has occurred."
				: message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
				: message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
				: string.Empty;

			string userId = User.Identity.GetUserId();
			var model = new IndexViewModel
			{
				HasPassword = this.HasPassword(),
				PhoneNumber = await this.UserManager.GetPhoneNumberAsync(userId),
				TwoFactor = await this.UserManager.GetTwoFactorEnabledAsync(userId),
				Logins = await this.UserManager.GetLoginsAsync(userId),
				BrowserRemembered = await this.AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
			};

			return this.View(model);
		}

		/// <summary>
		/// POST: /Manage/RemoveLogin
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
		{
			ManageMessageId? message;

			var result = await this.UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}

				message = ManageMessageId.RemoveLoginSuccess;
			}
			else
			{
				message = ManageMessageId.Error;
			}

			return this.RedirectToAction("ManageLogins", new { Message = message });
		}

		/// <summary>
		/// GET: /Manage/AddPhoneNumber
		/// </summary>
		[HttpGet]
		public ActionResult AddPhoneNumber()
		{
			return this.View();
		}

		/// <summary>
		/// POST: /Manage/AddPhoneNumber
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			// Generate the token and send it
			string code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);

			if (this.UserManager.SmsService != null)
			{
				var message = new IdentityMessage
				{
					Destination = model.Number,
					Body = "Your security code is: " + code
				};
				await this.UserManager.SmsService.SendAsync(message);
			}

			return this.RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
		}

		/// <summary>
		/// POST: /Manage/EnableTwoFactorAuthentication
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EnableTwoFactorAuthentication()
		{
			await this.UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);

			var user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
			}

			return this.RedirectToAction("Index", "Manage");
		}

		/// <summary>
		/// POST: /Manage/DisableTwoFactorAuthentication
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DisableTwoFactorAuthentication()
		{
			await this.UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);

			var user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
			}

			return this.RedirectToAction("Index", "Manage");
		}

		/// <summary>
		/// GET: /Manage/VerifyPhoneNumber
		/// </summary>
		[HttpGet]
		public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
		{
			await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);

			// Send an SMS through the SMS provider to verify the phone number
			return phoneNumber == null ? this.View("Error") : this.View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
		}

		/// <summary>
		/// POST: /Manage/VerifyPhoneNumber
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			var result = await this.UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
			if (result.Succeeded)
			{
				var user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}
				return this.RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
			}

			// If we got this far, something failed, redisplay form
			this.ModelState.AddModelError("", @"Failed to verify phone");

			return this.View(model);
		}

		/// <summary>
		/// POST: /Manage/RemovePhoneNumber
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> RemovePhoneNumber()
		{
			var result = await this.UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
			if (!result.Succeeded)
			{
				return this.RedirectToAction("Index", new { Message = ManageMessageId.Error });
			}

			var user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
			}

			return this.RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
		}

		/// <summary>
		/// GET: /Manage/ChangePassword
		/// </summary>
		[HttpGet]
		public ActionResult ChangePassword()
		{
			return this.View();
		}

		/// <summary>
		/// POST: /Manage/ChangePassword
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			var result = await this.UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
			if (result.Succeeded)
			{
				var user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}
				return this.RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
			}

			this.AddErrors(result);

			return this.View(model);
		}

		/// <summary>
		/// GET: /Manage/SetPassword
		/// </summary>
		[HttpGet]
		public ActionResult SetPassword()
		{
			return this.View();
		}

		/// <summary>
		/// POST: /Manage/SetPassword
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			var result = await this.UserManager.AddPasswordAsync(this.User.Identity.GetUserId(), model.NewPassword);
			if (result.Succeeded)
			{
				var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
				if (user != null)
				{
					await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}
				return this.RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
			}

			this.AddErrors(result);

			// If we got this far, something failed, redisplay form
			return this.View(model);
		}

		/// <summary>
		/// GET: /Manage/ManageLogins
		/// </summary>
		[HttpGet]
		public async Task<ActionResult> ManageLogins(ManageMessageId? message)
		{
			this.ViewBag.StatusMessage =
				message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
				: message == ManageMessageId.Error ? "An error has occurred."
				: "";

			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user == null)
			{
				return this.View("Error");
			}

			var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
			var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();

			this.ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;

			return this.View(new ManageLoginsViewModel
			{
				CurrentLogins = userLogins,
				OtherLogins = otherLogins
			});
		}

		/// <summary>
		/// POST: /Manage/LinkLogin
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkLogin(string provider)
		{
			// Request a redirect to the external login provider to link a login for the current user
			return new AccountController.ChallengeResult(provider, this.Url.Action("LinkLoginCallback", "Manage"), this.User.Identity.GetUserId());
		}

		/// <summary>
		/// GET: /Manage/LinkLoginCallback
		/// </summary>
		[HttpGet]
		public async Task<ActionResult> LinkLoginCallback()
		{
			var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, this.User.Identity.GetUserId());
			if (loginInfo == null)
			{
				return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
			}

			var result = await this.UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);

			return result.Succeeded ? this.RedirectToAction("ManageLogins") : this.RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
		}

		#region [Helpers]
		/// <summary>
		/// Enumerates the manage message ids.
		/// </summary>
		public enum ManageMessageId
		{
			AddPhoneSuccess,
			ChangePasswordSuccess,
			SetTwoFactorSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			RemovePhoneSuccess,
			Error
		}

		/// <summary>
		/// The XSRF key.
		/// Used for XSRF protection when adding external logins
		/// </summary>
		private const string XsrfKey = "XsrfId";

		/// <summary>
		/// Gets the authentication manager.
		/// </summary>
		/// <value>
		/// The authentication manager.
		/// </value>
		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return this.HttpContext.GetOwinContext().Authentication;
			}
		}

		/// <summary>
		/// Adds the errors.
		/// </summary>
		/// <param name="result">The result.</param>
		private void AddErrors(IdentityResult result)
		{
			foreach (string error in result.Errors)
				this.ModelState.AddModelError("", error);
		}

		/// <summary>
		/// Determines whether this instance has password.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this instance has password; otherwise, <c>false</c>.
		/// </returns>
		private bool HasPassword()
		{
			var user = this.UserManager.FindById(User.Identity.GetUserId());

			return user?.PasswordHash != null;
		}

		/// <summary>
		/// Determines whether [has phone number].
		/// </summary>
		/// <returns>
		///   <c>true</c> if [has phone number]; otherwise, <c>false</c>.
		/// </returns>
		// ReSharper disable once UnusedMember.Local
		private bool HasPhoneNumber()
		{
			var user = this.UserManager.FindById(User.Identity.GetUserId());

			return user?.PhoneNumber != null;
		}
		#endregion

		#region [CleanUp]
		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.userManagerField != null)
			{
				this.userManagerField.Dispose();
				this.userManagerField = null;
			}

			base.Dispose(disposing);
		}
		#endregion
	}
}