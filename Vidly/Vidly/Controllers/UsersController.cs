using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using Vidly.Models;
using Vidly.Identity;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
	[Authorize(Roles = ApplicationRoles.CanManageUsers)]
	public sealed class UsersController : BaseController
	{
		#region [Properties]
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;

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
				return this.signInManagerField ?? this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
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
				return this.userManagerField ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				this.userManagerField = value;
			}
		}
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="UsersController"/> class.
		/// </summary>
		public UsersController()
		{
			this.context = new ApplicationDbContext();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UsersController" /> class.
		/// </summary>
		/// 
		/// <param name="userManager">The user manager.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : this()
		{
			this.UserManager = userManager;
			this.SignInManager = signInManager;
		}
		#endregion

		/// <summary>
		/// GET: users/
		/// </summary>
		[HttpGet]
		[Route("users")]
		public async Task<ActionResult> Index()
		{
			var viewModels = new List<UserViewModel>();
			foreach (var user in await this.UserManager.Users.ToListAsync())
			{
				viewModels.Add(new UserViewModel
				{
					User = user,
					UserRoles = await this.UserManager.GetRolesAsync(user.Id)
				});
			}

			return this.User.IsInRole(ApplicationRoles.CanManageUsers)
				? this.View("List", viewModels)
				: this.View("ReadOnlyList", viewModels);
		}

		/// <summary>
		/// GET: users/details/id
		/// </summary>
		/// <param name="id">The user id.</param>
		[HttpGet]
		[Route("users/{id}")]
		public async Task<ActionResult> Details(string id)
		{
			var user = await this.UserManager.FindByIdAsync(id);
			if (user == null)
				return this.HttpNotFound();

			var viewModel = new UserViewModel
			{
				User = user,
				UserRoles = await this.UserManager.GetRolesAsync(user.Id)
			};

			return this.View("Details", viewModel);
		}

		/// <summary>
		/// GET: users/create
		/// </summary>
		[HttpGet]
		[Route("users/create")]
		public async Task<ActionResult> Create()
		{
			var viewModel = new UserFormViewModel
			{
				User = null,
				AvailableRoles = await this.context.Roles.Select(role => role.Name).ToListAsync()
			};

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// GET: users/edit/id
		/// </summary>
		/// <param name="id">The user id.</param>
		[HttpGet]
		[Route("users/edit/{id}")]
		public async Task<ActionResult> Edit(string id)
		{
			var user = await this.UserManager.FindByIdAsync(id);
			if (user == null)
				return this.HttpNotFound();

			var viewModel = new UserFormViewModel
			{
				User = user,
				UserRoles = await this.UserManager.GetRolesAsync(user.Id),
				AvailableRoles = await this.context.Roles.Select(role => role.Name).ToListAsync()
			};

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// POST: users/delete/id
		/// </summary>
		/// <param name="id">The user id.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(string id)
		{
			var user = await this.UserManager.FindByIdAsync(id);
			if (user == null)
				return this.HttpNotFound();

			await this.UserManager.DeleteAsync(user);

			this.TempData[MessageKey] = "User deleted successfully.";
			this.TempData[MessageTypeKey] = MessageTypeSuccess;

			return this.RedirectToAction("Index", "Users");
		}

		/// <summary>
		/// Saves the specified user.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="password">The new password.</param>
		/// <param name="userRoles">The role names.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Save(ApplicationUser user, string password, List<string> userRoles)
		{
			if (ModelState.IsValid == false)
			{
				var viewModel = new UserFormViewModel
				{
					User = user,
					UserRoles = userRoles,
					AvailableRoles = await this.context.Roles.Select(role => role.Name).ToListAsync()
				};

				return this.View("Form", viewModel);
			}

			var databaseUser = await this.UserManager.FindByIdAsync(user.Id);
			if (databaseUser == null)
			{
				// Create the user
				var result = await this.UserManager.CreateAsync(user, password);
				if (result.Succeeded == false)
					return this.HandleError(result);

				databaseUser = await this.UserManager.FindByIdAsync(user.Id);

				this.TempData[MessageKey] = "User created successfully.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}
			else
			{
				// Update the user
				databaseUser.FiscalNumber = user.FiscalNumber;
				databaseUser.Email = user.Email;
				databaseUser.EmailConfirmed = user.EmailConfirmed;
				databaseUser.PhoneNumber = user.PhoneNumber;
				databaseUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
				databaseUser.TwoFactorEnabled = user.TwoFactorEnabled;

				var result = await this.UserManager.UpdateAsync(databaseUser);
				if (result.Succeeded == false)
					return this.HandleError(result);

				this.TempData[MessageKey] = "User updated successfully.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}

			// Update the roles
			var roles = await this.UserManager.GetRolesAsync(databaseUser.Id);
			await this.UserManager.RemoveFromRolesAsync(databaseUser.Id, roles.ToArray());

			if (userRoles != null && userRoles.Count > 0)
			{
				foreach (string roleName in userRoles)
				{
					if (string.IsNullOrWhiteSpace(roleName))
						continue;

					await this.UserManager.AddToRoleAsync(databaseUser.Id, roleName);
				}
			}

			return this.RedirectToAction("Index", "Users");
		}

		/// <summary>
		/// Handles the error.
		/// </summary>
		/// <param name="result">The result.</param>
		private ViewResult HandleError(IdentityResult result)
		{
			this.ModelState.AddModelError(string.Empty, string.Join(Environment.NewLine, result.Errors));
			return this.View("Error");
		}

		#region [CleanUp]
		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			this.context.Dispose();
		}
		#endregion
	}
}