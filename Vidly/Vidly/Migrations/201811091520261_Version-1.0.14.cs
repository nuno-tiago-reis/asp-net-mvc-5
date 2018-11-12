namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version1014 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AddColumn("dbo.AspNetUsers", "FiscalNumber", c => c.String(nullable: false));
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.DropColumn("dbo.AspNetUsers", "FiscalNumber");
		}
	}
}
