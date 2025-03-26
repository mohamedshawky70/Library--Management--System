using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddAll : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_bookCategories_books_BookId",
				table: "bookCategories");

			migrationBuilder.DropForeignKey(
				name: "FK_bookCategories_categories_CategoryId",
				table: "bookCategories");

			migrationBuilder.DropForeignKey(
				name: "FK_books_authors_AuthorId",
				table: "books");

			migrationBuilder.DropForeignKey(
				name: "FK_copyBooks_books_BookId",
				table: "copyBooks");

			migrationBuilder.DropPrimaryKey(
				name: "PK_categories",
				table: "categories");

			migrationBuilder.DropPrimaryKey(
				name: "PK_books",
				table: "books");

			migrationBuilder.DropPrimaryKey(
				name: "PK_bookCategories",
				table: "bookCategories");

			migrationBuilder.DropPrimaryKey(
				name: "PK_authors",
				table: "authors");

			migrationBuilder.DropIndex(
				name: "IX_AspNetUsers_Email",
				table: "AspNetUsers");

			migrationBuilder.DropIndex(
				name: "IX_AspNetUsers_FullName",
				table: "AspNetUsers");

			migrationBuilder.DropPrimaryKey(
				name: "PK_copyBooks",
				table: "copyBooks");

			migrationBuilder.DropColumn(
				name: "CreatedById",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "CreatedOn",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "FullName",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "IsDeleted",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "LastUpdatedById",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "LastUpdatedOn",
				table: "AspNetUsers");

			migrationBuilder.RenameTable(
				name: "categories",
				newName: "Categories");

			migrationBuilder.RenameTable(
				name: "books",
				newName: "Books");

			migrationBuilder.RenameTable(
				name: "bookCategories",
				newName: "BookCategories");

			migrationBuilder.RenameTable(
				name: "authors",
				newName: "Authors");

			migrationBuilder.RenameTable(
				name: "copyBooks",
				newName: "CopiesBook");

			migrationBuilder.RenameIndex(
				name: "IX_books_AuthorId",
				table: "Books",
				newName: "IX_Books_AuthorId");

			migrationBuilder.RenameIndex(
				name: "IX_bookCategories_CategoryId",
				table: "BookCategories",
				newName: "IX_BookCategories_CategoryId");

			migrationBuilder.RenameIndex(
				name: "IX_copyBooks_BookId",
				table: "CopiesBook",
				newName: "IX_CopiesBook_BookId");

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "Categories",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(50)",
				oldMaxLength: 50);




			migrationBuilder.AddPrimaryKey(
				name: "PK_Categories",
				table: "Categories",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Books",
				table: "Books",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_BookCategories",
				table: "BookCategories",
				columns: new[] { "BookId", "CategoryId" });

			migrationBuilder.AddPrimaryKey(
				name: "PK_Authors",
				table: "Authors",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_CopiesBook",
				table: "CopiesBook",
				column: "Id");

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

			migrationBuilder.DropPrimaryKey(
				name: "PK_Categories",
				table: "Categories");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Books",
				table: "Books");

			migrationBuilder.DropPrimaryKey(
				name: "PK_BookCategories",
				table: "BookCategories");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Authors",
				table: "Authors");

			migrationBuilder.DropPrimaryKey(
				name: "PK_CopiesBook",
				table: "CopiesBook");

			migrationBuilder.RenameTable(
				name: "Categories",
				newName: "categories");

			migrationBuilder.RenameTable(
				name: "Books",
				newName: "books");

			migrationBuilder.RenameTable(
				name: "BookCategories",
				newName: "bookCategories");

			migrationBuilder.RenameTable(
				name: "Authors",
				newName: "authors");

			migrationBuilder.RenameTable(
				name: "CopiesBook",
				newName: "copyBooks");

			migrationBuilder.RenameIndex(
				name: "IX_Books_AuthorId",
				table: "books",
				newName: "IX_books_AuthorId");

			migrationBuilder.RenameIndex(
				name: "IX_BookCategories_CategoryId",
				table: "bookCategories",
				newName: "IX_bookCategories_CategoryId");

			migrationBuilder.RenameIndex(
				name: "IX_CopiesBook_BookId",
				table: "copyBooks",
				newName: "IX_copyBooks_BookId");

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "categories",
				type: "nvarchar(50)",
				maxLength: 50,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "AspNetUserTokens",
				type: "nvarchar(450)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(128)",
				oldMaxLength: 128);

			migrationBuilder.AlterColumn<string>(
				name: "LoginProvider",
				table: "AspNetUserTokens",
				type: "nvarchar(450)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(128)",
				oldMaxLength: 128);

			migrationBuilder.AddColumn<string>(
				name: "CreatedById",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "CreatedOn",
				table: "AspNetUsers",
				type: "datetime2",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<string>(
				name: "FullName",
				table: "AspNetUsers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<bool>(
				name: "IsDeleted",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<string>(
				name: "LastUpdatedById",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "LastUpdatedOn",
				table: "AspNetUsers",
				type: "datetime2",
				nullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "ProviderKey",
				table: "AspNetUserLogins",
				type: "nvarchar(450)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(128)",
				oldMaxLength: 128);

			migrationBuilder.AlterColumn<string>(
				name: "LoginProvider",
				table: "AspNetUserLogins",
				type: "nvarchar(450)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(128)",
				oldMaxLength: 128);

			migrationBuilder.AddPrimaryKey(
				name: "PK_categories",
				table: "categories",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_books",
				table: "books",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_bookCategories",
				table: "bookCategories",
				columns: new[] { "BookId", "CategoryId" });

			migrationBuilder.AddPrimaryKey(
				name: "PK_authors",
				table: "authors",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_copyBooks",
				table: "copyBooks",
				column: "Id");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_Email",
				table: "AspNetUsers",
				column: "Email",
				unique: true,
				filter: "[Email] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_FullName",
				table: "AspNetUsers",
				column: "FullName",
				unique: true);

			migrationBuilder.AddForeignKey(
				name: "FK_bookCategories_books_BookId",
				table: "bookCategories",
				column: "BookId",
				principalTable: "books",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_bookCategories_categories_CategoryId",
				table: "bookCategories",
				column: "CategoryId",
				principalTable: "categories",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_books_authors_AuthorId",
				table: "books",
				column: "AuthorId",
				principalTable: "authors",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_copyBooks_books_BookId",
				table: "copyBooks",
				column: "BookId",
				principalTable: "books",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
