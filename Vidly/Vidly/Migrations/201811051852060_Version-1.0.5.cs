namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version105 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AddColumn("dbo.MembershipTypes", "Name", c => c.String(maxLength: 255));

			this.Sql("UPDATE MembershipTypes SET Name = 'Pay as You Go' WHERE ID=1");
			this.Sql("UPDATE MembershipTypes SET Name = 'Monthly' WHERE ID=2");
			this.Sql("UPDATE MembershipTypes SET Name = 'Quarterly' WHERE ID=3");
			this.Sql("UPDATE MembershipTypes SET Name = 'Annual' WHERE ID=4");
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.AlterColumn("dbo.MembershipTypes", "Name", c => c.String(nullable: false));
		}
	}
}