using System.Collections.Generic;
using System.Web.Optimization;

namespace Vidly.Utility
{
	public static class BundleExtensions
	{
		public static Bundle NonOrdering(this Bundle bundle)
		{
			bundle.Orderer = new NonOrderingBundleOrderer();
			return bundle;
		}
	}

	public sealed class NonOrderingBundleOrderer : IBundleOrderer
	{
		public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
		{
			return files;
		}
	}
}