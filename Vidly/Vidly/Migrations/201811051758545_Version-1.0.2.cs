namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version102 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.CreateTable
			(
				"dbo.MembershipTypes",
				c => new
				{
					ID = c.Int(nullable: false, identity: true),
					SignUpFee = c.Short(nullable: false),
					DiscountRate = c.Byte(nullable: false),
					DurationInMonths = c.Byte(nullable: false),
				}
			)
			.PrimaryKey(t => t.ID);

			this.AddColumn("dbo.Customers", "MembershipTypeID", c => c.Int(nullable: false));
			this.CreateIndex("dbo.Customers", "MembershipTypeID");
			this.AddForeignKey("dbo.Customers", "MembershipTypeID", "dbo.MembershipTypes", "ID", cascadeDelete: true);
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.DropForeignKey("dbo.Customers", "MembershipTypeID", "dbo.MembershipTypes");
			this.DropIndex("dbo.Customers", new[] { "MembershipTypeID" });
			this.DropColumn("dbo.Customers", "MembershipTypeID");
			this.DropTable("dbo.MembershipTypes");
		}
	}
}
