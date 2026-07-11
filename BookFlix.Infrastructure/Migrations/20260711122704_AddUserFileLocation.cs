using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookFlix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFileLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileLocation",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("3dba3903-21a6-413d-a479-eb807eb5e6ed"),
                column: "FileLocation",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileLocation",
                table: "Users");
        }
    }
}
