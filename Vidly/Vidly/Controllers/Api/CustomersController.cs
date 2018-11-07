using AutoMapper;

using System;
using System.Net;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;

using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.API
{
	public sealed class CustomersController : ApiController
	{
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomersController"/> class.
		/// </summary>
		public CustomersController()
		{
			this.context = new ApplicationDbContext();
		}

		/// <summary>
		/// GET /api/customers
		/// </summary>
		[HttpGet]
		public IHttpActionResult GetCustomers()
		{
			return this.Ok(this.context.Customers.Include(customer => customer.MembershipType).AsEnumerable().Select(Mapper.Map<Customer, CustomerDto>));
		}

		/// <summary>
		/// GET /api/customers/id
		/// </summary>
		[HttpGet]
		public IHttpActionResult GetCustomer(int id)
		{
			var customer = this.context.Customers.SingleOrDefault(c => c.ID == id);
			if (customer == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return this.Ok(Mapper.Map<Customer, CustomerDto>(customer));
		}

		/// <summary>
		/// POST /api/customers
		/// </summary>
		[HttpPost]
		public IHttpActionResult CreateCustomer(CustomerDto customerDto)
		{
			if (ModelState.IsValid == false)
				return this.BadRequest();

			var customer = Mapper.Map<CustomerDto, Customer>(customerDto);

			this.context.Customers.Add(customer);
			this.context.SaveChanges();

			customerDto.ID = customer.ID;

			return this.Created(new Uri($"{Request.RequestUri}/{customer.ID}"), customerDto);
		}

		/// <summary>
		/// PUT /api/customers/id
		/// </summary>
		[HttpPut]
		public void UpdateCustomer(int id, CustomerDto customerDto)
		{
			if (ModelState.IsValid == false)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var customer = this.context.Customers.SingleOrDefault(c => c.ID == id);
			if (customer == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			Mapper.Map(customerDto, customer);
			this.context.SaveChanges();
		}

		/// <summary>
		/// DELETE /api/customers/id
		/// </summary>
		[HttpDelete]
		public void DeleteCustomer(int id)
		{
			if (ModelState.IsValid == false)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var customer = this.context.Customers.SingleOrDefault(c => c.ID == id);
			if (customer == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			this.context.Customers.Remove(customer);
			this.context.SaveChanges();
		}
	}
}