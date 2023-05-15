using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDelete_ContactLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_ContactLogs_Persons_PersonId",
                table: "ContactLogs",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactLogs_Persons_PersonId",
                table: "ContactLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactLogs_Persons_PersonId",
                table: "ContactLogs",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
