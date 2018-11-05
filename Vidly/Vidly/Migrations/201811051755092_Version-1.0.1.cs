namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version101 : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Customers", "IsSubscribedToNewsletter", c => c.Boolean(nullable: false));
		}

		public override void Down()
		{
			DropColumn("dbo.Customers", "IsSubscribedToNewsletter");
		}
	}
}
