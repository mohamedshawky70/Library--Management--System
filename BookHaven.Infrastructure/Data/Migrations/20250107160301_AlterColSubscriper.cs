using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AlterColSubscriper : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Img",
				table: "subscripers");

			migrationBuilder.AddColumn<string>(
				name: "ImgUrl",
				table: "subscripers",
				type: "nvarchar(max)",
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ImgUrl",
				table: "subscripers");

			migrationBuilder.AddColumn<string>(
				name: "Img",
				table: "subscripers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");
		}
	}
}
