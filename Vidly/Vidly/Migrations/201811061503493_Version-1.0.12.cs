namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version1012 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			AlterColumn("dbo.Movies", "Name", c => c.String(nullable: false, maxLength: 255));
		}

		/// <inheritdoc />
		public override void Down()
		{
			AlterColumn("dbo.Movies", "Name", c => c.String(nullable: false));
		}
	}
}
