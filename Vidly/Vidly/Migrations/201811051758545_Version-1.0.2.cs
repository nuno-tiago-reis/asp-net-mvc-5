namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version102 : DbMigration
	{
		public override void Up()
		{
			CreateTable(
					"dbo.MembershipTypes",
					c => new
					{
						ID = c.Int(nullable: false, identity: true),
						SignUpFee = c.Short(nullable: false),
						DiscountRate = c.Byte(nullable: false),
						DurationInMonths = c.Byte(nullable: false),
					})
				.PrimaryKey(t => t.ID);

			AddColumn("dbo.Customers", "MembershipTypeID", c => c.Int(nullable: false));
			CreateIndex("dbo.Customers", "MembershipTypeID");
			AddForeignKey("dbo.Customers", "MembershipTypeID", "dbo.MembershipTypes", "ID", cascadeDelete: true);
		}

		public override void Down()
		{
			DropForeignKey("dbo.Customers", "MembershipTypeID", "dbo.MembershipTypes");
			DropIndex("dbo.Customers", new[] { "MembershipTypeID" });
			DropColumn("dbo.Customers", "MembershipTypeID");
			DropTable("dbo.MembershipTypes");
		}
	}
}
