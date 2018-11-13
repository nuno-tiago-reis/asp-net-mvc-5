﻿using System;
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

		#region [Index]
		/// <summary>
		/// GET: /manage/
		/// </summary>
		public async Task<ActionResult> Index(ManageMessageId? message)
		{
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (message)
			{
				case ManageMessageId.ChangeEmailSuccess:
					this.ViewBag.StatusMessage = "Your email has been changed.";
					break;
				case ManageMessageId.ConfirmEmailSuccess:
					this.ViewBag.StatusMessage = "A confirmation email has been sent.";
					break;
				case ManageMessageId.ChangePasswordSuccess:
					this.ViewBag.StatusMessage = "Your password has been changed.";
					break;
				case ManageMessageId.ChangePhoneNumberSuccess:
					this.ViewBag.StatusMessage = "Your phone number has been changed.";
					break;
				case ManageMessageId.ConfirmPhoneNumberSuccess:
					this.ViewBag.StatusMessage = "A confirmation message has been sent.";
					break;
				case ManageMessageId.AddExternalLoginSuccess:
					this.ViewBag.StatusMessage = "The external login has been added.";
					break;
				case ManageMessageId.RemoveExternalLoginSuccess:
					this.ViewBag.StatusMessage = "The external login has been removed.";
					break;
				case ManageMessageId.Error:
				case null:
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(message), message, null);
			}

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
		public ActionResult ChangeEmail()
		{
			string userId = this.User.Identity.GetUserId();

			var viewModel = new ChangeEmailViewModel
			{
				Email = this.UserManager.Users.First(user => user.Id == userId).Email
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

			return this.RedirectToAction("Index", new { Message = ManageMessageId.ChangeEmailSuccess });
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

			return this.RedirectToAction("Index", new { Message = ManageMessageId.ConfirmEmailSuccess });
		}
		#endregion

		#region [Phone Number]
		/// <summary>
		/// GET: /manage/changephonenumber
		/// </summary>
		[HttpGet]
		public ActionResult ChangePhoneNumber()
		{
			string userId = this.User.Identity.GetUserId();

			var viewModel = new ChangePhoneNumberViewModel
			{
				PhoneNumber = this.UserManager.Users.First(user => user.Id == userId).PhoneNumber
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

			return this.RedirectToAction("Index", new { Message = ManageMessageId.ChangePhoneNumberSuccess });
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

			var user = this.UserManager.Users.First(u => u.Id == userId);

			// Create the confirmation token
			string token = await this.UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, user.PhoneNumber);
			string callbackUrl = this.Url.Action("ConfirmPhoneNumber", "Account", new { userId = user.Id, phoneNumber = user.PhoneNumber, token }, protocol: Request.Url?.Scheme);

			// Send the confirmation sms
			await this.UserManager.SendSmsAsync(user.Id, "Please confirm your phone number by clicking <a href=\"" + callbackUrl + "\">here</a>");

			return this.RedirectToAction("Index", new { Message = ManageMessageId.ConfirmPhoneNumberSuccess });
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
					return this.RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
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
					return this.RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
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
		public async Task<ActionResult> ExternalLogins(ManageMessageId? message)
		{
			this.ViewBag.StatusMessage =
				  message == ManageMessageId.AddExternalLoginSuccess ? "The external login was added."
				: message == ManageMessageId.RemoveExternalLoginSuccess ? "The external login was removed."
				: message == ManageMessageId.Error ? "An error has occurred."
				: string.Empty;

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
			ManageMessageId? message;

			var result = await this.UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}

				message = ManageMessageId.RemoveExternalLoginSuccess;
			}
			else
			{
				message = ManageMessageId.Error;
			}

			return this.RedirectToAction("ExternalLogins", new { Message = message });
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
				return RedirectToAction("ExternalLogins", new { Message = ManageMessageId.Error });
			}

			var result = await this.UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);

			return this.RedirectToAction("ExternalLogins", new { Message = result.Succeeded ? ManageMessageId.AddExternalLoginSuccess : ManageMessageId.Error });
		}
		#endregion

		#region [Helpers]
		/// <summary>
		/// Enumerates the manage message ids.
		/// </summary>
		public enum ManageMessageId
		{
			ChangeEmailSuccess,
			ConfirmEmailSuccess,
			ChangePasswordSuccess,
			ChangePhoneNumberSuccess,
			ConfirmPhoneNumberSuccess,

			AddExternalLoginSuccess,
			RemoveExternalLoginSuccess,

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