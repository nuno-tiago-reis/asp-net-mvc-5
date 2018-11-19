using System.Web.Mvc;
using System.Web.UI;

namespace Vidly.Controllers
{
	public class HomeController : BaseController
	{
		#region [Index]
		/// <summary>
		/// GET: /
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		[OutputCache(Duration = 50, Location = OutputCacheLocation.Server)]
		public ActionResult Index()
		{
			return this.View();
		}
		#endregion
	}
}