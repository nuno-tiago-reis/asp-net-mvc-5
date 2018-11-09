using System.Web.Mvc;

namespace Vidly.Controllers
{
	public class HomeController : Controller
	{
		/// <summary>
		/// GET: /
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Index()
		{
			return this.View();
		}
	}
}