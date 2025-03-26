using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class Add_Subscriper_Area_GovernorateTables : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_BookCategories_Books_BookId",
				table: "BookCategories");

			migrationBuilder.DropForeignKey(
				name: "FK_BookCategories_Categories_CategoryId",
				table: "BookCategories");

			migrationBuilder.DropForeignKey(
				name: "FK_Books_Authors_AuthorId",
				table: "Books");

			migrationBuilder.DropForeignKey(
				name: "FK_CopiesBook_Books_BookId",
				table: "CopiesBook");

			migrationBuilder.CreateTable(
				name: "governorates",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<int>(type: "int", maxLength: 60, nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_governorates", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "areas",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<int>(type: "int", maxLength: 60, nullable: false),
					GovernorateId = table.Column<int>(type: "int", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_areas", x => x.Id);
					table.ForeignKey(
						name: "FK_areas_governorates_GovernorateId",
						column: x => x.GovernorateId,
						principalTable: "governorates",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "subscripers",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					FName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
					LName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
					NationalId = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
					MobileNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
					HasWatsApp = table.Column<bool>(type: "bit", nullable: false),
					IsBlackList = table.Column<bool>(type: "bit", nullable: false),
					Img = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
					DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
					AreaId = table.Column<int>(type: "int", nullable: false),
					GovernorateId = table.Column<int>(type: "int", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_subscripers", x => x.Id);
					table.ForeignKey(
						name: "FK_subscripers_areas_AreaId",
						column: x => x.AreaId,
						principalTable: "areas",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_subscripers_governorates_GovernorateId",
						column: x => x.GovernorateId,
						principalTable: "governorates",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_areas_GovernorateId",
				table: "areas",
				column: "GovernorateId");

			migrationBuilder.CreateIndex(
				name: "IX_subscripers_AreaId",
				table: "subscripers",
				column: "AreaId");

			migrationBuilder.CreateIndex(
				name: "IX_subscripers_GovernorateId",
				table: "subscripers",
				column: "GovernorateId");

			migrationBuilder.AddForeignKey(
				name: "FK_BookCategories_Books_BookId",
				table: "BookCategories",
				column: "BookId",
				principalTable: "Books",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_BookCategories_Categories_CategoryId",
				table: "BookCategories",
				column: "CategoryId",
				principalTable: "Categories",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Books_Authors_AuthorId",
				table: "Books",
				column: "AuthorId",
				principalTable: "Authors",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_CopiesBook_Books_BookId",
				table: "CopiesBook",
				column: "BookId",
				principalTable: "Books",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_BookCategories_Books_BookId",
				table: "BookCategories");

			migrationBuilder.DropForeignKey(
				name: "FK_BookCategories_Categories_CategoryId",
				table: "BookCategories");

			migrationBuilder.DropForeignKey(
				name: "FK_Books_Authors_AuthorId",
				table: "Books");

			migrationBuilder.DropForeignKey(
				name: "FK_CopiesBook_Books_BookId",
				table: "CopiesBook");

			migrationBuilder.DropTable(
				name: "subscripers");

			migrationBuilder.DropTable(
				name: "areas");

			migrationBuilder.DropTable(
				name: "governorates");

			migrationBuilder.AddForeignKey(
				name: "FK_BookCategories_Books_BookId",
				table: "BookCategories",
				column: "BookId",
				principalTable: "Books",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_BookCategories_Categories_CategoryId",
				table: "BookCategories",
				column: "CategoryId",
				principalTable: "Categories",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Books_Authors_AuthorId",
				table: "Books",
				column: "AuthorId",
				principalTable: "Authors",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_CopiesBook_Books_BookId",
				table: "CopiesBook",
				column: "BookId",
				principalTable: "Books",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
