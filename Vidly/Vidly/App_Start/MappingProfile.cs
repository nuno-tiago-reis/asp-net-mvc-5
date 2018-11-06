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

			Mapper.CreateMap<Movie, MovieDto>();
			Mapper.CreateMap<MovieDto, Movie>();
		}
	}
}