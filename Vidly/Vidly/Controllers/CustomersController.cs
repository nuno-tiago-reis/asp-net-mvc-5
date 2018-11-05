using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels.Customers;

namespace Vidly.Controllers
{
	public sealed class CustomersController : Controller
	{
		/// <summary>
		/// The customers.
		/// </summary>
		private static readonly List<Customer> Customers = new List<Customer>
		{
			new Customer {ID = 1, Name = "Harry Potter"},
			new Customer {ID = 2, Name = "Ronald Weasley"},
			new Customer {ID = 3, Name = "Hermione Granger"},
			new Customer {ID = 4, Name = "Albus Dumbledore"},
			new Customer {ID = 5, Name = "Minerva McGonagall"},
		};

		/// <summary>
		/// GET: Customers/List
		/// </summary>
		[Route("customers")]
		public ViewResult List()
		{
			var viewModel = new ListViewModel
			{
				Customers = Customers
			};

			return this.View(viewModel);
		}

		/// <summary>
		/// GET: Customers/Details
		/// </summary>
		[Route("customers/details/{id:regex(\\d)}")]
		public ViewResult Details(int id)
		{
			var customer = Customers.FirstOrDefault(m => m.ID == id);

			return this.View(customer);
		}
	}
}