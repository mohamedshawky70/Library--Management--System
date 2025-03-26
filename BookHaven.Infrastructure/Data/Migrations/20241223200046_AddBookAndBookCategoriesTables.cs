using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddBookAndBookCategoriesTables : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "books",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
					Img = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Publisher = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Hall = table.Column<string>(type: "nvarchar(max)", nullable: false),
					IsAvailableForRental = table.Column<bool>(type: "bit", nullable: false),
					PublishingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					AuthorId = table.Column<int>(type: "int", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_books", x => x.Id);
					table.ForeignKey(
						name: "FK_books_authors_AuthorId",
						column: x => x.AuthorId,
						principalTable: "authors",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "bookCategories",
				columns: table => new
				{
					BookId = table.Column<int>(type: "int", nullable: false),
					CategoryId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_bookCategories", x => new { x.BookId, x.CategoryId });
					table.ForeignKey(
						name: "FK_bookCategories_books_BookId",
						column: x => x.BookId,
						principalTable: "books",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_bookCategories_categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "categories",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_bookCategories_CategoryId",
				table: "bookCategories",
				column: "CategoryId");

			migrationBuilder.CreateIndex(
				name: "IX_books_AuthorId",
				table: "books",
				column: "AuthorId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "bookCategories");

			migrationBuilder.DropTable(
				name: "books");
		}
	}
}
