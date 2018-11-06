namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version1010 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.AddColumn("dbo.Movies", "NumberInStock", c => c.Int(nullable: false));
			this.AddColumn("dbo.Movies", "DateAdded", c => c.DateTime(nullable: false));
			this.AddColumn("dbo.Movies", "ReleaseDate", c => c.DateTime(nullable: false));
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.DropColumn("dbo.Movies", "ReleaseDate");
			this.DropColumn("dbo.Movies", "DateAdded");
			this.DropColumn("dbo.Movies", "NumberInStock");
		}
	}
}
