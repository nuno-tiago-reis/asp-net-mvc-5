namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version1019 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String(nullable: false));
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String());
		}
	}
}
