namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version101 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AddColumn("dbo.Customers", "IsSubscribedToNewsletter", c => c.Boolean(nullable: false));
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.DropColumn("dbo.Customers", "IsSubscribedToNewsletter");
		}
	}
}
