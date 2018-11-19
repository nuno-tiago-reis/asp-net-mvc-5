namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;
	
	public partial class Version1018 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.RenameColumn(table: "dbo.Rentals", name: "Customer_ID", newName: "CustomerID");
			this.RenameColumn(table: "dbo.Rentals", name: "Movie_ID", newName: "MovieID");
			this.RenameIndex(table: "dbo.Rentals", name: "IX_Movie_ID", newName: "IX_MovieID");
			this.RenameIndex(table: "dbo.Rentals", name: "IX_Customer_ID", newName: "IX_CustomerID");
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.RenameIndex(table: "dbo.Rentals", name: "IX_CustomerID", newName: "IX_Customer_ID");
			this.RenameIndex(table: "dbo.Rentals", name: "IX_MovieID", newName: "IX_Movie_ID");
			this.RenameColumn(table: "dbo.Rentals", name: "MovieID", newName: "Movie_ID");
			this.RenameColumn(table: "dbo.Rentals", name: "CustomerID", newName: "Customer_ID");
		}
	}
}