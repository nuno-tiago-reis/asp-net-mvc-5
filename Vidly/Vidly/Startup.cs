using System.Diagnostics.CodeAnalysis;

using Owin;

namespace Vidly
{
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public partial class Startup
	{
		/// <summary>
		/// Configurations the specified application.
		/// </summary>
		/// <param name="app">The application.</param>
		public void Configuration(IAppBuilder app)
		{
			this.ConfigureAuth(app);
			this.ConfigureUsers();
		}
	}
}