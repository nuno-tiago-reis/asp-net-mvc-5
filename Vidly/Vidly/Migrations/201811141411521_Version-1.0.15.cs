namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version1015 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.CreateTable
			(
				"dbo.Rentals",
				c => new
				{
					ID = c.Int(nullable: false, identity: true),
					DateRented = c.DateTime(nullable: false),
					DateReturned = c.DateTime(),
					Customer_ID = c.Int(nullable: false),
					Movie_ID = c.Int(nullable: false),
				}
			)
			.PrimaryKey(t => t.ID)
			.ForeignKey("dbo.Customers", t => t.Customer_ID, cascadeDelete: true)
			.ForeignKey("dbo.Movies", t => t.Movie_ID, cascadeDelete: true)
			.Index(t => t.Customer_ID)
			.Index(t => t.Movie_ID);
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.DropForeignKey("dbo.Rentals", "Movie_ID", "dbo.Movies");
			this.DropForeignKey("dbo.Rentals", "Customer_ID", "dbo.Customers");
			this.DropIndex("dbo.Rentals", new[] { "Movie_ID" });
			this.DropIndex("dbo.Rentals", new[] { "Customer_ID" });
			this.DropTable("dbo.Rentals");
		}
	}
}