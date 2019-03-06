using System.Web.Optimization;

using Vidly.Utility;

namespace Vidly
{
	public sealed class BundleConfig
	{
		/// <summary>
		/// The library folder.
		/// </summary>
		public const string LibraryFolder = "~/Content/Libraries";

		/// <summary>
		/// The static folder.
		/// </summary>
		public const string StaticFolder = "~/Content/Static";

		/// <summary>
		/// Registers the bundles.
		/// </summary>
		/// 
		/// <param name="bundles">The bundles.</param>
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/js").NonOrdering().Include
			(
				$"{LibraryFolder}/jquery/js/jquery.min.js",
				$"{LibraryFolder}/jquery-validate/js/jquery.validate.min.js",
				$"{LibraryFolder}/jquery-validation-unobtrusive/js/jquery.validate.unobtrusive.min.js",
				$"{LibraryFolder}/popper/js/umd/popper.min.js",
				$"{LibraryFolder}/bootstrap/js/bootstrap.min.js",
				$"{LibraryFolder}/bootbox/js/bootbox.min.js",
				$"{LibraryFolder}/alertify/build/alertify.min.js",
				$"{LibraryFolder}/datatables/js/jquery.datatables.min.js",
				$"{LibraryFolder}/datatables/js/datatables.bootstrap4.min.js",
				$"{LibraryFolder}/intl-tel-input/js/intlTelInput.js",
				$"{LibraryFolder}/moment/js/moment.min.js",
				$"{LibraryFolder}/modernizr/js/modernizr.min.js",
				$"{LibraryFolder}/typeahead/js/bloodhound.min.js",
				$"{LibraryFolder}/typeahead/js/typeahead.jquery.min.js"
			));

			bundles.Add(new StyleBundle("~/bundles/css").NonOrdering().Include
			(
				$"{StaticFolder}/css/bootstrap-lumen.css",
				$"{StaticFolder}/css/bootstrap-theme.css",
				$"{StaticFolder}/css/typeahead.min.css",
				$"{LibraryFolder}/alertify/build/css/alertify.min.css",
				$"{LibraryFolder}/font-awesome/css/font-awesome.min.css",
				$"{LibraryFolder}/datatables/css/dataTables.bootstrap4.min.css",
				$"{LibraryFolder}/intl-tel-input/css/intlTelInput.min.css",
				$"{StaticFolder}/css/site.css"
			));
		}
	}
}