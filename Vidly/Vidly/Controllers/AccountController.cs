﻿using System;
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
	public class AccountController : Controller
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
		/// Initializes a new instance of the <see cref="AccountController"/> class.
		/// </summary>
		public AccountController()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AccountController"/> class.
		/// </summary>
		/// 
		/// <param name="userManager">The user manager.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : this()
		{
			this.UserManager = userManager;
			this.SignInManager = signInManager;
		}
		#endregion

		/// <summary>
		/// GET: /account/login
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			this.ViewBag.ReturnUrl = returnUrl;

			return this.View();
		}

		/// <summary>
		/// GET: /account/register
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Register()
		{
			return this.View();
		}

		/// <summary>
		/// GET: /account/registerconfirmation
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult RegisterConfirmation()
		{
			return this.View();
		}

		/// <summary>
		/// GET: /account/resetpassword
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult ResetPassword(string token)
		{
			return string.IsNullOrWhiteSpace(token) ? this.View("Error") : this.View();
		}

		/// <summary>
		/// GET: /account/resetpasswordconfirmation
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return this.View();
		}

		/// <summary>
		/// GET: /account/forgotpassword
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return this.View();
		}

		/// <summary>
		/// GET: /account/forgotpasswordconfirmation
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return this.View();
		}


		/// <summary>
		/// GET: /account/confirmemail
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return this.View("Error");
			}

			var result = await this.UserManager.ConfirmEmailAsync(userId, code);
			if (result.Succeeded)
			{
				return this.View("ConfirmEmail");
			}
			else
			{

				return this.View("Error");
			}
		}

		/// <summary>
		/// POST: /account/login
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			// Check if the user exists
			var user = await this.UserManager.FindByNameAsync(model.UserOrEmail) ?? await this.UserManager.FindByEmailAsync(model.UserOrEmail);
			if (user == null)
			{
				this.ModelState.AddModelError(string.Empty, @"Invalid login attempt.");
				return this.View(model);
			}

			// Check if the email is confirmed
			if (!UserManager.IsEmailConfirmed(user.Id))
			{
				this.ModelState.AddModelError(string.Empty, @"Invalid login attempt.");
				return this.View(model);
			}

			// Check if the user and password are valid
			var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: true);
			switch (result)
			{
				case SignInStatus.Success:
				{
					return this.RedirectToLocal(returnUrl);
				}

				case SignInStatus.LockedOut:
				{
					return this.View("Lockout");
				}

				case SignInStatus.RequiresVerification:
				{
					return this.RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
				}

				case SignInStatus.Failure:
				{
					this.ModelState.AddModelError(string.Empty, @"Invalid login attempt.");
					return this.View(model);
				}

				default:
					throw new ArgumentOutOfRangeException(nameof(result), result, null);
			}
		}

		/// <summary>
		/// POST: /account/register
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			var user = new ApplicationUser { UserName = model.User, Email = model.Email };

			// Create the user
			var result = await this.UserManager.CreateAsync(user, model.Password);
			if (result.Succeeded == false)
			{
				this.AddErrors(result);
				return this.View(model);
			}

			// Create the confirmation code
			string code = await this.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
			string callbackUrl = this.Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = code }, protocol: Request.Url?.Scheme);

			// Send the confirmation email
			await this.UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

			// Redirect to home
			return this.RedirectToAction("RegisterConfirmation", "Account");
		}

		/// <summary>
		/// POST: /account/forgotpassword
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			// Check if the user exists and is confirmed
			var user = await this.UserManager.FindByNameAsync(model.UserOrEmail) ?? await this.UserManager.FindByEmailAsync(model.UserOrEmail);
			if (user == null || await this.UserManager.IsEmailConfirmedAsync(user.Id) == false)
			{
				// Don't reveal that the user does not exist
				return this.View("ForgotPasswordConfirmation");
			}

			// Create the confirmation token
			string token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
			string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token }, protocol: Request.Url?.Scheme);

			// Send the confirmation email
			await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

			// Redirect to forgot password confirmation
			return this.RedirectToAction("ForgotPasswordConfirmation", "Account");
		}

		/// <summary>
		/// GET: /account/resetpassword
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			// Check if the user exists and is confirmed
			var user = await this.UserManager.FindByNameAsync(model.UserOrEmail) ?? await this.UserManager.FindByEmailAsync(model.UserOrEmail);
			if (user == null || await this.UserManager.IsEmailConfirmedAsync(user.Id) == false)
			{
				// Don't reveal that the user does not exist
				return this.RedirectToAction("ResetPasswordConfirmation", "Account");
			}

			// Reset the password
			var result = await UserManager.ResetPasswordAsync(user.Id, model.Token, model.Password);
			if (result.Succeeded == false)
			{
				this.AddErrors(result);
				return this.View();
			}

			// Redirect to reset password confirmation
			return this.RedirectToAction("ResetPasswordConfirmation", "Account");
		}

		/// <summary>
		/// POST: /account/logoff
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

			return this.RedirectToAction("Index", "Home");
		}

		#region [LATER]
		/// <summary>
		/// GET: /Account/SendCode
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
		{
			string userId = await this.SignInManager.GetVerifiedUserIdAsync();

			if (userId == null)
			{
				return this.View("Error");
			}

			var userFactors = await this.UserManager.GetValidTwoFactorProvidersAsync(userId);
			var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();

			return this.View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		/// <summary>
		/// POST: /Account/SendCode
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SendCode(SendCodeViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View();
			}

			// Generate the token and send it
			if (!await this.SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
			{
				return this.View("Error");
			}
			return this.RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
		}

		/// <summary>
		/// GET: /Account/VerifyCode
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
		{
			// Require that the user has already logged in via username/password or external login
			if (!await this.SignInManager.HasBeenVerifiedAsync())
			{
				return this.View("Error");
			}
			return this.View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		/// <summary>
		/// POST: /Account/VerifyCode
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			// The following code protects for brute force attacks against the two factor codes. 
			// If a user enters incorrect codes for a specified amount of time then the user account 
			// will be locked out for a specified amount of time. 
			// You can configure the account lockout settings in IdentityConfig
			var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);

			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (result)
			{
				case SignInStatus.Success:
					return this.RedirectToLocal(model.ReturnUrl);

				case SignInStatus.LockedOut:
					return this.View("Lockout");

				case SignInStatus.Failure:
					this.ModelState.AddModelError("", @"Invalid code.");
					return this.View(model);

				default:
					throw new ArgumentOutOfRangeException(nameof(result), result, null);
			}
		}


		/// <summary>
		/// POST: /Account/ExternalLogin
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, this.Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}

		/// <summary>
		/// GET: /Account/ExternalLoginCallback
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return this.RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var result = await this.SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
			switch (result)
			{
				case SignInStatus.Success:
					return this.RedirectToLocal(returnUrl);

				case SignInStatus.LockedOut:
					return this.View("Lockout");

				case SignInStatus.RequiresVerification:
					return this.RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });

				case SignInStatus.Failure:
					// If the user does not have an account, then prompt the user to create an account
					this.ViewBag.ReturnUrl = returnUrl;
					this.ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
					return this.View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });

				default:
					throw new ArgumentOutOfRangeException(nameof(result), result, null);
			}
		}

		/// <summary>
		/// POST: /Account/ExternalLoginConfirmation
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		{
			if (this.User.Identity.IsAuthenticated)
			{
				return this.RedirectToAction("Index", "Manage");
			}

			if (this.ModelState.IsValid)
			{
				// Get the information about the user from the external login provider
				var info = await AuthenticationManager.GetExternalLoginInfoAsync();
				if (info == null)
				{
					return View("ExternalLoginFailure");
				}

				var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
				var result = await UserManager.CreateAsync(user);
				if (result.Succeeded)
				{
					result = await UserManager.AddLoginAsync(user.Id, info.Login);
					if (result.Succeeded)
					{
						await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
						return RedirectToLocal(returnUrl);
					}
				}

				this.AddErrors(result);
			}

			this.ViewBag.ReturnUrl = returnUrl;

			return this.View(model);
		}

		/// <summary>
		/// GET: /Account/ExternalLoginFailure
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return this.View();
		}
		#endregion

		#region [Helpers]
		/// <summary>
		/// The XSRF key.
		/// Used for XSRF protection when adding external logins
		/// </summary>
		private const string XsrfKey = "XsrfId";

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
		/// Redirects to local.
		/// </summary>
		/// <param name="returnUrl">The return URL.</param>
		/// <returns></returns>
		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (this.Url.IsLocalUrl(returnUrl))
			{
				return this.Redirect(returnUrl);
			}

			return this.RedirectToAction("Index", "Home");
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			/// <summary>
			/// Gets or sets the login provider.
			/// </summary>
			/// <value>
			/// The login provider.
			/// </value>
			public string LoginProvider { get; set; }

			/// <summary>
			/// Gets or sets the redirect URI.
			/// </summary>
			/// <value>
			/// The redirect URI.
			/// </value>
			public string RedirectUri { get; set; }

			/// <summary>
			/// Gets or sets the user identifier.
			/// </summary>
			/// <value>
			/// The user identifier.
			/// </value>
			public string UserId { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="ChallengeResult"/> class.
			/// </summary>
			/// <param name="provider">The provider.</param>
			/// <param name="redirectUri">The redirect URI.</param>
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="ChallengeResult"/> class.
			/// </summary>
			/// <param name="provider">The provider.</param>
			/// <param name="redirectUri">The redirect URI.</param>
			/// <param name="userId">The user identifier.</param>
			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				this.LoginProvider = provider;
				this.RedirectUri = redirectUri;
				this.UserId = userId;
			}

			/// <inheritdoc />
			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };

				if (this.UserId != null)
				{
					properties.Dictionary[XsrfKey] = this.UserId;
				}

				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion

		#region [CleanUp]
		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.userManagerField != null)
				{
					this.userManagerField.Dispose();
					this.userManagerField = null;
				}

				if (this.signInManagerField != null)
				{
					this.signInManagerField.Dispose();
					this.signInManagerField = null;
				}
			}

			base.Dispose(disposing);
		}
		#endregion
	}
}