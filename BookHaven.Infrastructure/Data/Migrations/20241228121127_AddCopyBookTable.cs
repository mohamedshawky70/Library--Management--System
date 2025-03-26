using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddCopyBookTable : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateSequence<int>(
				name: "Serial",
				startValue: 100L);

			migrationBuilder.CreateTable(
				name: "copyBooks",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					IsAvailableForRental = table.Column<bool>(type: "bit", nullable: false),
					EditionNumber = table.Column<byte>(type: "tinyint", nullable: false),
					SerialNumber = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR Serial"),
					BookId = table.Column<int>(type: "int", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_copyBooks", x => x.Id);
					table.ForeignKey(
						name: "FK_copyBooks_books_BookId",
						column: x => x.BookId,
						principalTable: "books",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_copyBooks_BookId",
				table: "copyBooks",
				column: "BookId",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "copyBooks");

			migrationBuilder.DropSequence(
				name: "Serial");
		}
	}
}
