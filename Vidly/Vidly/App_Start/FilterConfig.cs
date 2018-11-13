using System.Web.Mvc;

namespace Vidly
{
	public sealed class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new AuthorizeAttribute());
			filters.Add(new HandleErrorAttribute());
			filters.Add(new RequireHttpsAttribute());
		}
	}
}