using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Remove_CompanyProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyProfileBlobUrl",
                table: "Companies");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileBlobUrl",
                table: "Persons",
                type: "nvarchar(1028)",
                maxLength: 1028,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WebUrl",
                table: "Companies",
                type: "nvarchar(1028)",
                maxLength: 1028,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Companies");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileBlobUrl",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1028)",
                oldMaxLength: 1028,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WebUrl",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1028)",
                oldMaxLength: 1028,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyProfileBlobUrl",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
