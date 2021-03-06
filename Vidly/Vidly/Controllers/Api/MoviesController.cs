﻿using AutoMapper;

using System;
using System.Net;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;

using Vidly.Contracts;
using Vidly.Models;

namespace Vidly.Controllers.API
{
	[Authorize]
	public sealed class MoviesController : ApiController
	{
		#region [Properties]
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="MoviesController"/> class.
		/// </summary>
		public MoviesController()
		{
			this.context = new ApplicationDbContext();
		}
		#endregion

		/// <summary>
		/// GET /api/movies
		/// </summary>
		[HttpGet]
		public IHttpActionResult GetMovies(string query = null, bool includeOutOfStock = true)
		{
			var movies = this.context.Movies.Include(m => m.Genre);

			if (string.IsNullOrWhiteSpace(query) == false)
				movies = movies.Where(m => m.Name.Contains(query));

			if (includeOutOfStock == false)
				movies = movies.Where(m => m.NumberInStock > m.NumberRented);

			return this.Ok(movies.AsEnumerable().Select(Mapper.Map<Movie, MovieDto>));
		}

		/// <summary>
		/// GET /api/movies/id
		/// </summary>
		[HttpGet]
		public IHttpActionResult GetMovie(int id)
		{
			var movie = this.context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.ID == id);
			if (movie == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return this.Ok(Mapper.Map<Movie, MovieDto>(movie));
		}

		/// <summary>
		/// POST /api/movies
		/// </summary>
		[HttpPost]
		[Authorize(Roles = ApplicationRoles.CanManageMovies)]
		public IHttpActionResult CreateMovie(MovieDto movieDto)
		{
			if (this.ModelState.IsValid == false)
				return this.BadRequest();

			var movie = Mapper.Map<MovieDto, Movie>(movieDto);

			this.context.Movies.Add(movie);
			this.context.SaveChanges();

			movieDto.ID = movie.ID;

			return this.Created(new Uri($"{Request.RequestUri}/{movie.ID}"), movieDto);
		}

		/// <summary>
		/// PUT /api/movies/id
		/// </summary>
		[HttpPut]
		[Authorize(Roles = ApplicationRoles.CanManageMovies)]
		public void UpdateMovie(int id, MovieDto movieDto)
		{
			if (this.ModelState.IsValid == false)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var movie = this.context.Movies.SingleOrDefault(m => m.ID == id);
			if (movie == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			Mapper.Map(movieDto, movie);
			this.context.SaveChanges();
		}

		/// <summary>
		/// DELETE /api/movies/id
		/// </summary>
		[HttpDelete]
		[Authorize(Roles = ApplicationRoles.CanManageMovies)]
		public void DeleteMovie(int id)
		{
			if (this.ModelState.IsValid == false)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var movie = this.context.Movies.SingleOrDefault(c => c.ID == id);
			if (movie == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			this.context.Movies.Remove(movie);
			this.context.SaveChanges();
		}
	}
}