using System.Linq;
using System.Web.Mvc;

using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{	
	[Authorize]
	public sealed class CustomersController : BaseController
	{
		#region [Properties]
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomersController" /> class.
		/// </summary>
		public CustomersController()
		{
			this.context = new ApplicationDbContext();
		}
		#endregion

		/// <summary>
		/// GET: customers/
		/// </summary>
		[HttpGet]
		[Route("customers")]
		public ViewResult Index()
		{
			return this.User.IsInRole(ApplicationRoles.CanManageCustomers)
				? this.View("List")
				: this.View("ReadOnlyList");
		}

		/// <summary>
		/// GET: customers/details/id
		/// </summary>
		/// <param name="id">The customer id.</param>
		[HttpGet]
		[Route("customers/{id}")]
		public ActionResult Details(int id)
		{
			var customer = this.context.Customers.Include(nameof(Customer.MembershipType)).FirstOrDefault(c => c.ID == id);

			if (customer == null)
				return this.HttpNotFound();

			return this.View("Details", customer);
		}

		/// <summary>
		/// GET: customers/create
		/// </summary>
		[HttpGet]
		[Route("customers/create")]
		[Authorize(Roles = ApplicationRoles.CanManageCustomers)]
		public ViewResult Create()
		{
			var viewModel = new CustomerFormViewModel
			{
				MembershipTypes = this.context.MembershipTypes
			};

			//if (System.Runtime.Caching.MemoryCache.Default[nameof(this.context.MembershipTypes)] == null)
			//	System.Runtime.Caching.MemoryCache.Default[nameof(this.context.MembershipTypes)] = this.context.MembershipTypes;

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// GET: customers/edit/id
		/// </summary>
		/// <param name="id">The customer id.</param>
		[HttpGet]
		[Route("customers/edit/{id:regex(\\d)}")]
		[Authorize(Roles = ApplicationRoles.CanManageCustomers)]
		public ActionResult Edit(int id)
		{
			var customer = this.context.Customers.FirstOrDefault(c => c.ID == id);
			if (customer == null)
				return this.HttpNotFound();

			var viewModel = new CustomerFormViewModel
			{
				Customer = customer,
				MembershipTypes = this.context.MembershipTypes
			};

			//if (System.Runtime.Caching.MemoryCache.Default[nameof(this.context.MembershipTypes)] == null)
			//	System.Runtime.Caching.MemoryCache.Default[nameof(this.context.MembershipTypes)] = this.context.MembershipTypes;

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// POST: customers/delete/id
		/// </summary>
		/// <param name="id">The customer id.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = ApplicationRoles.CanManageCustomers)]
		public ActionResult Delete(int id)
		{
			var customer = this.context.Customers.FirstOrDefault(c => c.ID == id);
			if (customer == null)
				return this.HttpNotFound();

			this.context.Customers.Remove(customer);
			this.context.SaveChanges();

			this.TempData[MessageKey] = "Customer deleted successfully.";
			this.TempData[MessageTypeKey] = MessageTypeSuccess;

			return this.RedirectToAction("Index", "Customers");
		}

		/// <summary>
		/// Saves the specified customer.
		/// </summary>
		/// <param name="customer">The customer.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = ApplicationRoles.CanManageCustomers)]
		public ActionResult Save(Customer customer)
		{
			if (ModelState.IsValid == false)
			{
				var viewModel = new CustomerFormViewModel
				{
					Customer = customer,
					MembershipTypes = this.context.MembershipTypes
				};

				return this.View("Form", viewModel);
			}

			if (customer.ID == 0)
			{
				this.context.Customers.Add(customer);

				this.TempData[MessageKey] = "Customer created successfully.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}
			else
			{
				var databaseCustomer = this.context.Customers.FirstOrDefault(c => c.ID == customer.ID);
				if (databaseCustomer == null)
					return this.HttpNotFound();

				this.TryUpdateModel(databaseCustomer, nameof(Customer), new[]
				{
					nameof(Customer.Name),
					nameof(Customer.BirthDate),
					nameof(Customer.MembershipTypeID),
					nameof(Customer.IsSubscribedToNewsletter)
				});

				this.TempData[MessageKey] = "Customer updated successfully.";
				this.TempData[MessageTypeKey] = MessageTypeSuccess;
			}

			this.context.SaveChanges();

			return this.RedirectToAction("Index", "Customers");
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