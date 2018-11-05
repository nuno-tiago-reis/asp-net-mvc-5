namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version103 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.Sql("SET IDENTITY_INSERT dbo.MembershipTypes ON");

			this.Sql("INSERT INTO dbo.MembershipTypes (ID, SignUpFee, DurationInMonths, DiscountRate) VALUES (1, 0, 0, 0)");
			this.Sql("INSERT INTO dbo.MembershipTypes (ID, SignUpFee, DurationInMonths, DiscountRate) VALUES (2, 30, 1, 10)");
			this.Sql("INSERT INTO dbo.MembershipTypes (ID, SignUpFee, DurationInMonths, DiscountRate) VALUES (3, 90, 3, 15)");
			this.Sql("INSERT INTO dbo.MembershipTypes (ID, SignUpFee, DurationInMonths, DiscountRate) VALUES (4, 300, 12, 20)");

			this.Sql("SET IDENTITY_INSERT dbo.MembershipTypes OFF");
		}

		/// <inheritdoc />
		public override void Down()
		{
		}
	}
}
