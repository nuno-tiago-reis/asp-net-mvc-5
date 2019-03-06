using AutoMapper;

using Vidly.Contracts;
using Vidly.Models;

namespace Vidly.App_Start
{
	public sealed class MappingProfile : Profile
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MappingProfile"/> class.
		/// </summary>
		public MappingProfile()
		{
			this.CreateMap<Customer, CustomerDto>();
			this.CreateMap<CustomerDto, Customer>();

			this.CreateMap<MembershipTypeDto, MembershipType>();
			this.CreateMap<MembershipType, MembershipTypeDto>();

			this.CreateMap<Movie, MovieDto>();
			this.CreateMap<MovieDto, Movie>();

			this.CreateMap<GenreDto, Genre>();
			this.CreateMap<Genre, GenreDto>();

			this.CreateMap<RentalDto, Rental>();
			this.CreateMap<Rental, RentalDto>();

			this.CreateMap<UserDto, ApplicationUser>();
			this.CreateMap<ApplicationUser, UserDto>();
		}
	}
}