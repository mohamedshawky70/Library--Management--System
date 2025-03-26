using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddCopyBookTable_1 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_copyBooks_BookId",
				table: "copyBooks");

			migrationBuilder.CreateIndex(
				name: "IX_copyBooks_BookId",
				table: "copyBooks",
				column: "BookId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_copyBooks_BookId",
				table: "copyBooks");

			migrationBuilder.CreateIndex(
				name: "IX_copyBooks_BookId",
				table: "copyBooks",
				column: "BookId",
				unique: true);
		}
	}
}
