using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddIndex_Subscriper_Area_GovernorateTables : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_areas_GovernorateId",
				table: "areas");

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "governorates",
				type: "nvarchar(60)",
				maxLength: 60,
				nullable: false,
				oldClrType: typeof(int),
				oldType: "int",
				oldMaxLength: 60);

			migrationBuilder.CreateIndex(
				name: "IX_subscripers_Email",
				table: "subscripers",
				column: "Email",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_subscripers_MobileNumber",
				table: "subscripers",
				column: "MobileNumber",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_subscripers_NationalId",
				table: "subscripers",
				column: "NationalId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_governorates_Name",
				table: "governorates",
				column: "Name",
				unique: true);

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

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_subscripers_Email",
				table: "subscripers");

			migrationBuilder.DropIndex(
				name: "IX_subscripers_MobileNumber",
				table: "subscripers");

			migrationBuilder.DropIndex(
				name: "IX_subscripers_NationalId",
				table: "subscripers");

			migrationBuilder.DropIndex(
				name: "IX_governorates_Name",
				table: "governorates");

			migrationBuilder.DropIndex(
				name: "IX_areas_GovernorateId",
				table: "areas");

			migrationBuilder.DropIndex(
				name: "IX_areas_Name",
				table: "areas");

			migrationBuilder.AlterColumn<int>(
				name: "Name",
				table: "governorates",
				type: "int",
				maxLength: 60,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(60)",
				oldMaxLength: 60);

			migrationBuilder.CreateIndex(
				name: "IX_areas_GovernorateId",
				table: "areas",
				column: "GovernorateId");
		}
	}
}
