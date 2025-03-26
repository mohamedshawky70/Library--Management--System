using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class RemoveIndexFromFullNameApplicationUserTable : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_AspNetUsers_FullName",
				table: "AspNetUsers");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_FullName",
				table: "AspNetUsers",
				column: "FullName",
				unique: true);
		}
	}
}
