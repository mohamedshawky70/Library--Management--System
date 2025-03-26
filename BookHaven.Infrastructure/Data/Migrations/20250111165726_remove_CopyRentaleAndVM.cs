using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class remove_CopyRentaleAndVM : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Rental_subscripers_SubscriperId",
				table: "Rental");

			migrationBuilder.DropTable(
				name: "copyRentals");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Rental",
				table: "Rental");

			migrationBuilder.RenameTable(
				name: "Rental",
				newName: "Rentals");

			migrationBuilder.RenameIndex(
				name: "IX_Rental_SubscriperId",
				table: "Rentals",
				newName: "IX_Rentals_SubscriperId");

			migrationBuilder.AddColumn<DateTime>(
				name: "EndDate",
				table: "Rentals",
				type: "datetime2",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "ReturnDate",
				table: "Rentals",
				type: "datetime2",
				nullable: true);

			migrationBuilder.AddPrimaryKey(
				name: "PK_Rentals",
				table: "Rentals",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Rentals_subscripers_SubscriperId",
				table: "Rentals",
				column: "SubscriperId",
				principalTable: "subscripers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Rentals_subscripers_SubscriperId",
				table: "Rentals");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Rentals",
				table: "Rentals");

			migrationBuilder.DropColumn(
				name: "EndDate",
				table: "Rentals");

			migrationBuilder.DropColumn(
				name: "ReturnDate",
				table: "Rentals");

			migrationBuilder.RenameTable(
				name: "Rentals",
				newName: "Rental");

			migrationBuilder.RenameIndex(
				name: "IX_Rentals_SubscriperId",
				table: "Rental",
				newName: "IX_Rental_SubscriperId");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Rental",
				table: "Rental",
				column: "Id");

			migrationBuilder.CreateTable(
				name: "copyRentals",
				columns: table => new
				{
					RentalId = table.Column<int>(type: "int", nullable: false),
					CopyBookId = table.Column<int>(type: "int", nullable: false),
					EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					ExtendedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RentalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_copyRentals", x => new { x.RentalId, x.CopyBookId });
					table.ForeignKey(
						name: "FK_copyRentals_CopiesBook_CopyBookId",
						column: x => x.CopyBookId,
						principalTable: "CopiesBook",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_copyRentals_Rental_RentalId",
						column: x => x.RentalId,
						principalTable: "Rental",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_copyRentals_CopyBookId",
				table: "copyRentals",
				column: "CopyBookId");

			migrationBuilder.AddForeignKey(
				name: "FK_Rental_subscripers_SubscriperId",
				table: "Rental",
				column: "SubscriperId",
				principalTable: "subscripers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}
