using System.Web.Optimization;

namespace Vidly
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/lib").Include
			(
				"~/Scripts/jquery-{version}.js",
				"~/Scripts/bootstrap.js",
				"~/Scripts/bootbox.js",
				"~/Scripts/datatables/jquery.datatables.js",
				"~/Scripts/datatables/datatables.bootstrap.js"
			));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include
			(
				"~/Scripts/jquery.validate*"
			));

			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include
			(
				"~/Scripts/modernizr-*"
			));
			bundles.Add(new StyleBundle("~/Content/css").Include
			(
				"~/Content/bootstrap-lumen.css",
				"~/Content/datatables/css/datatables.bootstrap.css",
				"~/Content/site.css"
			));

		}
	}
}
