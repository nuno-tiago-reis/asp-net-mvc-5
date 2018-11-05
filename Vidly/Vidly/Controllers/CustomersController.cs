using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

using Vidly.Models;

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
		[Route("customers")]
		public ViewResult Index()
		{
			var customers = this.context.Customers.Include(c => c.MembershipType);

			return this.View(customers);
		}

		/// <summary>
		/// GET: Customers/Details
		/// </summary>
		[Route("customers/details/{id:regex(\\d)}")]
		public ActionResult Details(int id)
		{
			var customer = this.context.Customers.Include(c => c.MembershipType).FirstOrDefault(c => c.ID == id);

			if (customer == null)
				return this.HttpNotFound();

			return this.View(customer);
		}
	}
}