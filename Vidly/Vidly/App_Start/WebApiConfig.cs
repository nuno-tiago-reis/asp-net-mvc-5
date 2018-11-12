using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Vidly
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Modify the json serialization settings
			var settings = config.Formatters.JsonFormatter.SerializerSettings;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			// Map the routes defined in annotations
			config.MapHttpAttributeRoutes();

			// Map the default route
			config.Routes.MapHttpRoute
			(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
