namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version107 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AddColumn("dbo.Customers", "BirthDate", c => c.DateTime());
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.DropColumn("dbo.Customers", "BirthDate");
		}
	}
}
