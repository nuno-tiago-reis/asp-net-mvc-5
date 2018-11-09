using System.Linq;
using System.Web.Mvc;

using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
	[Authorize]
	public sealed class CustomersController : Controller
	{
		#region [Properties]
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Vidly.Controllers.CustomersController" /> class.
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
			return this.View();
		}

		/// <summary>
		/// GET: customers/create
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
		/// GET: customers/edit
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

		#region [CleanUp]
		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			this.context.Dispose();
		}
		#endregion
	}
}