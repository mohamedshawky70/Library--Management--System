using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddRenta_CopyRentaleTable : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Rental",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					PenaltyPaid = table.Column<bool>(type: "bit", nullable: false),
					SubscriperId = table.Column<int>(type: "int", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Rental", x => x.Id);
					table.ForeignKey(
						name: "FK_Rental_subscripers_SubscriperId",
						column: x => x.SubscriperId,
						principalTable: "subscripers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "copyRentals",
				columns: table => new
				{
					CopyBookId = table.Column<int>(type: "int", nullable: false),
					RentalId = table.Column<int>(type: "int", nullable: false)
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

			migrationBuilder.CreateIndex(
				name: "IX_Rental_SubscriperId",
				table: "Rental",
				column: "SubscriperId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "copyRentals");

			migrationBuilder.DropTable(
				name: "Rental");
		}
	}
}
