using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AlterColArea : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "areas",
				type: "nvarchar(60)",
				maxLength: 60,
				nullable: false,
				oldClrType: typeof(int),
				oldType: "int",
				oldMaxLength: 60);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<int>(
				name: "Name",
				table: "areas",
				type: "int",
				maxLength: 60,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(60)",
				oldMaxLength: 60);
		}
	}
}
