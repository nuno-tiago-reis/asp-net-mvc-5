namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version1017 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AddColumn("dbo.Movies", "NumberRented", c => c.Int(nullable: false));

			this.Sql("UPDATE Movies SET NumberRented = NumberInStock - NumberAvailable");

			this.DropColumn("dbo.Movies", "NumberAvailable");
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.AddColumn("dbo.Movies", "NumberAvailable", c => c.Int(nullable: false));

			this.Sql("UPDATE Movies SET NumberAvailable = NumberInStock - NumberRented");

			this.DropColumn("dbo.Movies", "NumberRented");
		}
	}
}