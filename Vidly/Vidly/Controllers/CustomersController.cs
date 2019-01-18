using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using Vidly.Identity;
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

		/// <summary>
		/// The sign in manager field.
		/// </summary>
		private ApplicationSignInManager signInManagerField;

		/// <summary>
		/// The user manager field.
		/// </summary>
		private ApplicationUserManager userManagerField;

		/// <summary>
		/// Gets the sign in manager.
		/// </summary>
		public ApplicationSignInManager SignInManager
		{
			get
			{
				return this.signInManagerField ?? this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				this.signInManagerField = value;
			}
		}

		/// <summary>
		/// Gets the user manager.
		/// </summary>
		public ApplicationUserManager UserManager
		{
			get
			{
				return this.userManagerField ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				this.userManagerField = value;
			}
		}
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomersController" /> class.
		/// </summary>
		public CustomersController()
		{
			this.context = new ApplicationDbContext();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomersController" /> class.
		/// </summary>
		/// 
		/// <param name="userManager">The user manager.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public CustomersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : this()
		{
			this.UserManager = userManager;
			this.SignInManager = signInManager;
		}
		#endregion

		/// <summary>
		/// GET: customers/
		/// </summary>
		[HttpGet]
		[Route("customers")]
		public async Task<ActionResult> Index()
		{
			return await this.UserManager.IsInRoleAsync(this.HttpContext.User.Identity.GetUserId(), ApplicationRoles.CanManageCustomers)
				? this.View("List")
				: this.View("ReadOnlyList");
		}

		/// <summary>
		/// GET: customers/details/id
		/// </summary>
		/// <param name="id">The customer id.</param>
		[HttpGet]
		[Route("customers/{id}")]
		public async Task<ActionResult> Details(int id)
		{
			var customer = await this.context.Customers.Include(nameof(Customer.MembershipType)).FirstOrDefaultAsync(c => c.ID == id);
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
		public async Task<ActionResult> Create()
		{
			var viewModel = new CustomerFormViewModel
			{
				MembershipTypes = await this.context.MembershipTypes.ToListAsync()
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
		public async Task<ActionResult> Edit(int id)
		{
			var customer = await this.context.Customers.FirstOrDefaultAsync(c => c.ID == id);
			if (customer == null)
				return this.HttpNotFound();

			var viewModel = new CustomerFormViewModel
			{
				Customer = customer,
				MembershipTypes = await this.context.MembershipTypes.ToListAsync()
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
		public async Task<ActionResult> Delete(int id)
		{
			var customer = await this.context.Customers.FirstOrDefaultAsync(c => c.ID == id);
			if (customer == null)
				return this.HttpNotFound();

			this.context.Customers.Remove(customer);
			await this.context.SaveChangesAsync();

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
		public async Task<ActionResult> Save(Customer customer)
		{
			if (ModelState.IsValid == false)
			{
				var viewModel = new CustomerFormViewModel
				{
					Customer = customer,
					MembershipTypes = await this.context.MembershipTypes.ToListAsync()
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
				var databaseCustomer = await this.context.Customers.FirstOrDefaultAsync(c => c.ID == customer.ID);
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

			await this.context.SaveChangesAsync();

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