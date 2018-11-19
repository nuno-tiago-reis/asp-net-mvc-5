using AutoMapper;

using Vidly.Dtos;
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
			Mapper.CreateMap<Customer, CustomerDto>();
			Mapper.CreateMap<CustomerDto, Customer>();

			Mapper.CreateMap<MembershipTypeDto, MembershipType>();
			Mapper.CreateMap<MembershipType, MembershipTypeDto>();

			Mapper.CreateMap<Movie, MovieDto>();
			Mapper.CreateMap<MovieDto, Movie>();

			Mapper.CreateMap<GenreDto, Genre>();
			Mapper.CreateMap<Genre, GenreDto>();

			Mapper.CreateMap<RentalDto, Rental>();
			Mapper.CreateMap<Rental, RentalDto>();

			Mapper.CreateMap<UserDto, ApplicationUser>();
			Mapper.CreateMap<ApplicationUser, UserDto>();
		}
	}
}