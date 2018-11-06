using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
	public sealed class CustomersController : Controller
	{
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;

		/// <inheritdoc />
		/// 
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Vidly.Controllers.CustomersController" /> class.
		/// </summary>
		public CustomersController()
		{
			this.context = new ApplicationDbContext();
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			this.context.Dispose();
		}

		/// <summary>
		/// GET: Customers/List
		/// </summary>
		[HttpGet]
		[Route("customers")]
		public ViewResult Index()
		{
			var customers = this.context.Customers.Include(c => c.MembershipType);

			return this.View(customers);
		}

		/// <summary>
		/// GET: Customers/Details
		/// </summary>
		[HttpGet]
		[Route("customers/details/{id:regex(\\d)}")]
		public ActionResult Details(int id)
		{
			var customer = this.context.Customers.Include(c => c.MembershipType).FirstOrDefault(c => c.ID == id);

			if (customer == null)
				return this.HttpNotFound();

			return this.View(customer);
		}

		/// <summary>
		/// GET: Customers/Create
		/// </summary>
		[HttpGet]
		[Route("customers/create")]
		public ViewResult Create()
		{
			var viewModel = new CustomerFormViewModel
			{
				MembershipTypes = this.context.MembershipTypes
			};

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// GET: Customers/Edit
		/// </summary>
		[HttpGet]
		[Route("customers/edit/{id:regex(\\d)}")]
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

			return this.View("Form", viewModel);
		}

		/// <summary>
		/// Deletes the customer with the specified ID.
		/// </summary>
		/// <param name="id">The customer id.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			var customer = this.context.Customers.FirstOrDefault(c => c.ID == id);

			if (customer == null)
				return this.HttpNotFound();

			this.context.Customers.Remove(customer);
			this.context.SaveChanges();

			return this.RedirectToAction("Index", "Customers");
		}

		/// <summary>
		/// Saves the specified customer.
		/// </summary>
		/// <param name="customer">The customer.</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
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
			}
			else
			{
				var databaseCustomer = this.context.Customers.FirstOrDefault(c => c.ID == customer.ID);

				this.TryUpdateModel(databaseCustomer, nameof(Customer), new[]
				{
					nameof(Customer.Name),
					nameof(Customer.BirthDate),
					nameof(Customer.MembershipTypeID),
					nameof(Customer.IsSubscribedToNewsletter)
				});
			}

			this.context.SaveChanges();

			return this.RedirectToAction("Index", "Customers");
		}
	}
}