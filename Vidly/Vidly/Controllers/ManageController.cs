using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Vidly.Identity;
using Vidly.Models;

namespace Vidly.Controllers
{
	[Authorize]
	public class ManageController : BaseController
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

		#region [Index]
		/// <summary>
		/// GET: /manage/
		/// </summary>
		public async Task<ActionResult> Index()
		{
			string userId = User.Identity.GetUserId();

			var model = new IndexViewModel
			{
				User = await this.UserManager.Users.FirstOrDefaultAsync(user => user.Id == userId),
				UserRoles = await this.UserManager.GetRolesAsync(userId),
				UserExternalLogins = await this.UserManager.GetLoginsAsync(userId),
				BrowserRemembered = await this.AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
			};

			return this.View(model);
		}
		#endregion

		#region [Email]
		/// <summary>
		/// GET: /manage/changeemail
		/// </summary>
		[HttpGet]
		public async Task<ActionResult> ChangeEmail()
		{
			string userId = this.User.Identity.GetUserId();

			var viewModel = new ChangeEmailViewModel
			{
				Email = (await this.UserManager.Users.FirstAsync(user => user.Id == userId)).Email
			};

			return this.View(viewModel);
		}

		/// <summary>
		/// POST: /manage/changeemail
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			string userId = User.Identity.GetUserId();

			var result = await this.UserManager.SetEmailAsync(userId, model.Email);
			if (result.Succeeded == false)
			{
				this.AddErrors(result);
				return this.View(model);
			}

			this.TempData[MessageKey] = "Your email has been changed.";
			this.TempData[MessageTypeKey] = MessageTypeSuccess;

			return this.RedirectToAction("Index");
		}

		/// <summary>
		/// POST: /manage/confirmemail
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ConfirmEmail(string email)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View("Index");
			}

			string userId = User.Identity.GetUserId();

			// Create the confirmation token
			string token = await this.UserManager.GenerateEmailConfirmationTokenAsync(userId);
			string callbackUrl = this.Url.Action("ConfirmEmail", "Account", new { userId, token }, protocol: Request.Url?.Scheme);

			// Send the confirmation email
			await this.UserManager.SendEmailAsync(userId, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

			this.TempData[MessageKey] = "A confirmation email has been sent.";
			this.TempData[MessageTypeKey] = MessageTypeSuccess;

			return this.RedirectToAction("Index");
		}
		#endregion

		#region [Phone Number]
		/// <summary>
		/// GET: /manage/changephonenumber
		/// </summary>
		[HttpGet]
		public async Task<ActionResult> ChangePhoneNumber()
		{
			string userId = this.User.Identity.GetUserId();

			var viewModel = new ChangePhoneNumberViewModel
			{
				PhoneNumber = (await this.UserManager.Users.FirstAsync(user => user.Id == userId)).PhoneNumber
			};

			return this.View(viewModel);
		}

		/// <summary>
		/// POST: /manage/changephonenumber
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePhoneNumber(ChangePhoneNumberViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			var result = await this.UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber);
			if (result.Succeeded == false)
			{
				this.AddErrors(result);
				return this.View(model);
			}

			this.TempData[MessageKey] = "Your phone number has been changed.";
			this.TempData[MessageTypeKey] = MessageTypeSuccess;

			return this.RedirectToAction("Index");
		}

		/// <summary>
		/// POST: /manage/confirmphonenumber
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ConfirmPhoneNumber(string phoneNumber)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View("Index");
			}

			string userId = this.User.Identity.GetUserId();

			var user = await this.UserManager.Users.FirstAsync(u => u.Id == userId);

			// Create the confirmation token
			string token = await this.UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, user.PhoneNumber);
			string callbackUrl = this.Url.Action("ConfirmPhoneNumber", "Account", new { userId = user.Id, phoneNumber = user.PhoneNumber, token }, protocol: Request.Url?.Scheme);

			// Send the confirmation sms
			await this.UserManager.SendSmsAsync(user.Id, "Please confirm your phone number by clicking <a href=\"" + callbackUrl + "\">here</a>");

			this.TempData[MessageKey] = "A confirmation message has been sent.";
			this.TempData[MessageTypeKey] = MessageTypeSuccess;

			return this.RedirectToAction("Index");
		}
		#endregion

		#region [Change Password]
		/// <summary>
		/// GET: /manage/changepassword
		/// </summary>
		[HttpGet]
		public ActionResult ChangePassword()
		{
			var viewModel = new ChangePasswordViewModel
			{
				HasPassword = this.HasPassword()
			};

			return this.View(viewModel);
		}

		/// <summary>
		/// POST: /manage/changepassword
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			if (model.HasPassword)
			{
				var result = await this.UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
				if (result.Succeeded)
				{
					var user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
					if (user != null)
					{
						await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
					}

					this.TempData[MessageKey] = "Your password has been changed.";
					this.TempData[MessageTypeKey] = MessageTypeSuccess;

					return this.RedirectToAction("Index");
				}

				this.AddErrors(result);
			}
			else
			{
				var result = await this.UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
				if (result.Succeeded)
				{
					var user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
					if (user != null)
					{
						await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
					}

					this.TempData[MessageKey] = "Your password has been changed.";
					this.TempData[MessageTypeKey] = MessageTypeSuccess;

					return this.RedirectToAction("Index");
				}

				this.AddErrors(result);
			}

			return this.View(model);
		}
		#endregion

		#region [Manage External Logins]
		/// <summary>
		/// GET: /manage/logins
		/// </summary>
		[HttpGet]
		public async Task<ActionResult> ExternalLogins()
		{
			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user == null)
			{
				return this.View("Error");
			}

			var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
			var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();

			this.ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;

			return this.View(new ExternalLoginsViewModel
			{
				RegisteredLogins = userLogins,
				UnregisteredLogins = otherLogins
			});
		}

		/// <summary>
		/// POST: /manage/addlogin
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddLogin(string provider)
		{
			// Request a redirect to the external login provider to link a login for the current user
			return new AccountController.ChallengeResult(provider, this.Url.Action("AddLoginCallback", "Manage"), this.User.Identity.GetUserId());
		}

		/// <summary>
		/// POST: /manage/removelogin
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
		{
			var result = await this.UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}

				this.TempData[MessageKey] = "The external login was removed.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}
			else
			{
				this.TempData[MessageKey] = "An error has occured.";
				this.TempData[MessageTypeKey] = MessageTypeError;
			}

			return this.RedirectToAction("ExternalLogins");
		}

		/// <summary>
		/// GET: /manage/addlogincallback
		/// </summary>
		[HttpGet]
		public async Task<ActionResult> AddLoginCallback()
		{
			var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, this.User.Identity.GetUserId());
			if (loginInfo == null)
			{
				this.TempData[MessageKey] = "An error has occured.";
				this.TempData[MessageTypeKey] = MessageTypeError;

				return RedirectToAction("ExternalLogins");
			}

			var result = await this.UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
			if (result.Succeeded)
			{
				this.TempData[MessageKey] = "The external login was added.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}
			else
			{
				this.TempData[MessageKey] = "An error has occured.";
				this.TempData[MessageTypeKey] = MessageTypeError;
			}

			return this.RedirectToAction("ExternalLogins");
		}
		#endregion

		#region [Two Factor Authentication]
		/// <summary>
		/// POST: /manage/enabletwofactorauthentication
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EnableTwoFactorAuthentication()
		{
			await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);

			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
			}

			this.TempData[MessageKey] = "Two factor authentication has been enabled.";
			this.TempData[MessageTypeKey] = MessageTypeSuccess;

			return this.RedirectToAction("Index", "Manage");
		}

		/// <summary>
		/// POST: /manage/disabletwofactorauthentication
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DisableTwoFactorAuthentication()
		{
			await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);

			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
			}

			this.TempData[MessageKey] = "Two factor authentication has been disabled.";
			this.TempData[MessageTypeKey] = MessageTypeSuccess;

			return this.RedirectToAction("Index", "Manage");
		}
		#endregion

		#region [Helpers]
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