using System.Diagnostics.CodeAnalysis;

namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	internal sealed class Configuration : DbMigrationsConfiguration<Models.ApplicationDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(Models.ApplicationDbContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.
		}
	}
}
