namespace Vidly.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Version108 : DbMigration
	{
		/// <inheritdoc />
		public override void Up()
		{
			this.CreateTable
			(
				"dbo.Genres",
				c => new
				{
					ID = c.Int(nullable: false, identity: true),
					Name = c.String(nullable: false, maxLength: 255)
				}
			)
			.PrimaryKey(t => t.ID);

			this.AddColumn("dbo.Movies", "GenreID", c => c.Int(nullable: false));
			this.CreateIndex("dbo.Movies", "GenreID");
			this.AddForeignKey("dbo.Movies", "GenreID", "dbo.Genres", "ID", cascadeDelete: true);
		}

		/// <inheritdoc />
		public override void Down()
		{
			this.DropForeignKey("dbo.Movies", "GenreID", "dbo.Genres");
			this.DropIndex("dbo.Movies", new[] { "GenreID" });
			this.DropColumn("dbo.Movies", "GenreID");
			this.DropTable("dbo.Genres");
		}
	}
}
