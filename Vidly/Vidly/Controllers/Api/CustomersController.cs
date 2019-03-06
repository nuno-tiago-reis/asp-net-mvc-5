using AutoMapper;

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
	public sealed class CustomersController : ApiController
	{
		#region [Properties]
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomersController"/> class.
		/// </summary>
		public CustomersController()
		{
			this.context = new ApplicationDbContext();
		}
		#endregion

		/// <summary>
		/// GET /api/customers
		/// </summary>
		[HttpGet]
		public IHttpActionResult GetCustomers(string query = null)
		{
			var customers = this.context.Customers.Include(c => c.MembershipType);

			if (string.IsNullOrWhiteSpace(query) == false)
				customers = customers.Where(c => c.Name.Contains(query));

			return this.Ok(customers.AsEnumerable().Select(Mapper.Map<Customer, CustomerDto>));
		}

		/// <summary>
		/// GET /api/customers/id
		/// </summary>
		[HttpGet]
		public IHttpActionResult GetCustomer(int id)
		{
			var customer = this.context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.ID == id);
			if (customer == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return this.Ok(Mapper.Map<Customer, CustomerDto>(customer));
		}

		/// <summary>
		/// POST /api/customers
		/// </summary>
		[HttpPost]
		[Authorize(Roles = ApplicationRoles.CanManageCustomers)]
		public IHttpActionResult CreateCustomer(CustomerDto customerDto)
		{
			if (this.ModelState.IsValid == false)
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
		[Authorize(Roles = ApplicationRoles.CanManageCustomers)]
		public void UpdateCustomer(int id, CustomerDto customerDto)
		{
			if (this.ModelState.IsValid == false)
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
		[Authorize(Roles = ApplicationRoles.CanManageCustomers)]
		public void DeleteCustomer(int id)
		{
			if (this.ModelState.IsValid == false)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var customer = this.context.Customers.SingleOrDefault(c => c.ID == id);
			if (customer == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			this.context.Customers.Remove(customer);
			this.context.SaveChanges();
		}
	}
}