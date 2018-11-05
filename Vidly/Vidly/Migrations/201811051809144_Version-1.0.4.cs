namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version104 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 255));
			this.AlterColumn("dbo.Movies", "Name", c => c.String(nullable: false));
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.AlterColumn("dbo.Movies", "Name", c => c.String());
			this.AlterColumn("dbo.Customers", "Name", c => c.String());
		}
	}
}
