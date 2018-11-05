namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version109 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.Sql("SET IDENTITY_INSERT dbo.Genres ON");

			this.Sql("INSERT INTO dbo.Genres (ID, Name) VALUES (1, 'Action')");
			this.Sql("INSERT INTO dbo.Genres (ID, Name) VALUES (2, 'Adventure')");
			this.Sql("INSERT INTO dbo.Genres (ID, Name) VALUES (3, 'Thriller')");
			this.Sql("INSERT INTO dbo.Genres (ID, Name) VALUES (4, 'Fantasy')");
			this.Sql("INSERT INTO dbo.Genres (ID, Name) VALUES (5, 'Romance')");
			this.Sql("INSERT INTO dbo.Genres (ID, Name) VALUES (6, 'Family')");
			this.Sql("INSERT INTO dbo.Genres (ID, Name) VALUES (7, 'Comedy')");

			this.Sql("SET IDENTITY_INSERT dbo.Genres OFF");
		}

		/// <inheritdoc />
		public override void Down()
		{
		}
	}
}