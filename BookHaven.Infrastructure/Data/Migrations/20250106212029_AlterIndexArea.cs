using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AlterIndexArea : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_areas_GovernorateId",
				table: "areas");

			migrationBuilder.DropIndex(
				name: "IX_areas_Name",
				table: "areas");

			migrationBuilder.CreateIndex(
				name: "IX_areas_GovernorateId",
				table: "areas",
				column: "GovernorateId");

			migrationBuilder.CreateIndex(
				name: "IX_areas_Name_GovernorateId",
				table: "areas",
				columns: new[] { "Name", "GovernorateId" },
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_areas_GovernorateId",
				table: "areas");

			migrationBuilder.DropIndex(
				name: "IX_areas_Name_GovernorateId",
				table: "areas");

			migrationBuilder.CreateIndex(
				name: "IX_areas_GovernorateId",
				table: "areas",
				column: "GovernorateId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_areas_Name",
				table: "areas",
				column: "Name",
				unique: true);
		}
	}
}
