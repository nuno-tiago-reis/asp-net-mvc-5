namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version1011 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AlterColumn("dbo.Customers", "BirthDate", c => c.DateTime(nullable: false));
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.AlterColumn("dbo.Customers", "BirthDate", c => c.DateTime());
		}
	}
}
