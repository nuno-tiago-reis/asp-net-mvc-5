using AutoMapper;

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using Vidly.Dtos;
using Vidly.Identity;
using Vidly.Models;

namespace Vidly.Controllers.API
{
	[Authorize]
	public sealed class UsersController : ApiController
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
				return this.signInManagerField ?? this.Request.GetOwinContext().Get<ApplicationSignInManager>();
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
				return this.userManagerField ?? this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
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
		/// GET /api/users
		/// </summary>
		[HttpGet]
		public IHttpActionResult GetUsers()
		{
			var userDtos = this.context.Users.AsEnumerable().Select(Mapper.Map<ApplicationUser, UserDto>).ToList();

			foreach (var userDto in userDtos)
				userDto.Roles = this.UserManager.GetRoles(userDto.Id);

			return this.Ok(userDtos);
		}

		/// <summary>
		/// DELETE /api/users/id
		/// </summary>
		[HttpDelete]
		[Authorize(Roles = ApplicationRoles.CanManageUsers)]
		public void Delete(string id)
		{
			if (this.ModelState.IsValid == false)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var user = this.UserManager.FindById(id);
			if (user == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			this.UserManager.Delete(user);
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