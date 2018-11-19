namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version1016 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AddColumn("dbo.Movies", "NumberAvailable", c => c.Int(nullable: false));

			this.Sql("UPDATE Movies SET NumberAvailable = NumberInStock");
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.DropColumn("dbo.Movies", "NumberAvailable");
		}
	}
}