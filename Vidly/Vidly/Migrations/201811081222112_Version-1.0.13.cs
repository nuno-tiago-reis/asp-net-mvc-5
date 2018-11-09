namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version1013 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AlterColumn("dbo.AspNetUsers", "Email", c => c.String(nullable: false, maxLength: 256));
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.AlterColumn("dbo.AspNetUsers", "Email", c => c.String(maxLength: 256));
		}
	}
}