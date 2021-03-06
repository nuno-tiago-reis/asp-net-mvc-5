﻿using System;
using AutoMapper;

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using Vidly.Contracts;
using Vidly.Identity;
using Vidly.Models;

namespace Vidly.Controllers.API
{
	[Authorize(Roles = ApplicationRoles.CanManageUsers)]
	public sealed class UsersController : ApiController
	{
		#region [Properties]
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;

		/// <summary>
		/// The user manager field.
		/// </summary>
		private ApplicationUserManager userManagerField;

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
		public UsersController(ApplicationUserManager userManager) : this()
		{
			this.UserManager = userManager;
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
		/// GET /api/users/id
		/// </summary>
		[HttpGet]
		public IHttpActionResult GetUser(string id)
		{
			var user = this.context.Users.SingleOrDefault(u => u.Id == id);
			if (user == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			var userDto = Mapper.Map<ApplicationUser, UserDto>(user);
			userDto.Roles = this.UserManager.GetRoles(userDto.Id);

			return this.Ok();
		}

		/// <summary>
		/// POST /api/users
		/// </summary>
		[HttpPost]
		public IHttpActionResult CreateUser(UserDto userDto)
		{
			if (this.ModelState.IsValid == false)
				return this.BadRequest();

			if (string.IsNullOrWhiteSpace(userDto.UserName))
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			if (string.IsNullOrWhiteSpace(userDto.Email))
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			if(string.IsNullOrWhiteSpace(userDto.PhoneNumber))
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			if(string.IsNullOrWhiteSpace(userDto.FiscalNumber))
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var user = Mapper.Map<UserDto, ApplicationUser>(userDto);

			var result = this.UserManager.Create(user);
			if (result.Succeeded == false)
			{
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}

			userDto.Id = user.Id;

			return this.Created(new Uri($"{Request.RequestUri}/{user.Id}"), userDto);
		}

		/// <summary>
		/// PUT /api/users/id
		/// </summary>
		[HttpPut]
		public void UpdateUser(string id, UserDto userDto)
		{
			if (this.ModelState.IsValid == false)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var user = this.context.Users.SingleOrDefault(u => u.Id == id);
			if (user == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			user.FiscalNumber = userDto.FiscalNumber;
			user.Email = userDto.Email;
			user.EmailConfirmed = userDto.EmailConfirmed;
			user.PhoneNumber = userDto.PhoneNumber;
			user.PhoneNumberConfirmed = userDto.PhoneNumberConfirmed;
			user.TwoFactorEnabled = userDto.TwoFactorEnabled;

			var result = this.UserManager.Update(user);
			if (result.Succeeded == false)
			{
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}
		}

		/// <summary>
		/// DELETE /api/users/id
		/// </summary>
		[HttpDelete]
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