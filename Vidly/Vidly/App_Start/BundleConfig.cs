using System.Web.Optimization;

namespace Vidly
{
	public sealed class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/lib").Include
			(
				"~/Scripts/jquery-{version}.js",
				"~/Scripts/bootstrap.js",
				"~/Scripts/bootbox.js",
				"~/Scripts/moment.js",
				"~/Scripts/toastr.js",
				"~/Scripts/typeahead.bundle.js",
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

			bundles.Add(new StyleBundle("~/content/css").Include
			(
				"~/Content/Styles/bootstrap-lumen.css",
				"~/Content/Styles/bootstrap-theme.css",
				"~/Content/DataTables/css/datatables.bootstrap.css",
				"~/Content/Styles/typeahead.css",
				"~/content/Styles/toastr.css",
				"~/Content/Styles/Site.css"
			));

		}
	}
}