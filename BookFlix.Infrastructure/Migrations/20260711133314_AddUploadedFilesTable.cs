using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookFlix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUploadedFilesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileLocation",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FileLocation",
                table: "Books");

            migrationBuilder.AddColumn<Guid>(
                name: "FileID",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FileID",
                table: "Books",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000001"),
                column: "FileID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000002"),
                column: "FileID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000003"),
                column: "FileID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000004"),
                column: "FileID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000005"),
                column: "FileID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000006"),
                column: "FileID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000007"),
                column: "FileID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000008"),
                column: "FileID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("3dba3903-21a6-413d-a479-eb807eb5e6ed"),
                column: "FileID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FileID",
                table: "Users",
                column: "FileID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_FileID",
                table: "Books",
                column: "FileID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_UploadedFiles_FileID",
                table: "Books",
                column: "FileID",
                principalTable: "UploadedFiles",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UploadedFiles_FileID",
                table: "Users",
                column: "FileID",
                principalTable: "UploadedFiles",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_UploadedFiles_FileID",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UploadedFiles_FileID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UploadedFiles");

            migrationBuilder.DropIndex(
                name: "IX_Users_FileID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Books_FileID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "FileID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FileID",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "FileLocation",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileLocation",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000001"),
                column: "FileLocation",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000002"),
                column: "FileLocation",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000003"),
                column: "FileLocation",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000004"),
                column: "FileLocation",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000005"),
                column: "FileLocation",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000006"),
                column: "FileLocation",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000007"),
                column: "FileLocation",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ID",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000008"),
                column: "FileLocation",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("3dba3903-21a6-413d-a479-eb807eb5e6ed"),
                column: "FileLocation",
                value: null);
        }
    }
}
