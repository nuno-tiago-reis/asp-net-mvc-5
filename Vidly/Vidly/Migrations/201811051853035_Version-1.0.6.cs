namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version106 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AlterColumn("dbo.MembershipTypes", "Name", c => c.String(nullable: false, maxLength: 255));
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.AlterColumn("dbo.MembershipTypes", "Name", c => c.String(maxLength: 255));
		}
	}
}
