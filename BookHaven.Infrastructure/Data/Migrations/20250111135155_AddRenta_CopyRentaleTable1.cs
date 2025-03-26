using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddRenta_CopyRentaleTable1 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "ExtendedOn",
				table: "copyRentals",
				type: "datetime2",
				nullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "RentalDate",
				table: "copyRentals",
				type: "datetime2",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "ReturnDate",
				table: "copyRentals",
				type: "datetime2",
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ExtendedOn",
				table: "copyRentals");

			migrationBuilder.DropColumn(
				name: "RentalDate",
				table: "copyRentals");

			migrationBuilder.DropColumn(
				name: "ReturnDate",
				table: "copyRentals");
		}
	}
}
