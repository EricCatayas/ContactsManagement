using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ContactLogs_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactLog_ContactTags_TagId",
                table: "ContactLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactLog_Persons_PersonId",
                table: "ContactLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactLog",
                table: "ContactLog");

            migrationBuilder.DropIndex(
                name: "IX_ContactLog_TagId",
                table: "ContactLog");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "ContactLog");

            migrationBuilder.RenameTable(
                name: "ContactLog",
                newName: "ContactLogs");

            migrationBuilder.RenameIndex(
                name: "IX_ContactLog_PersonId",
                table: "ContactLogs",
                newName: "IX_ContactLogs_PersonId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ContactLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactLogs",
                table: "ContactLogs",
                column: "LogId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactLogs_Persons_PersonId",
                table: "ContactLogs",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactLogs_Persons_PersonId",
                table: "ContactLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactLogs",
                table: "ContactLogs");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ContactLogs");

            migrationBuilder.RenameTable(
                name: "ContactLogs",
                newName: "ContactLog");

            migrationBuilder.RenameIndex(
                name: "IX_ContactLogs_PersonId",
                table: "ContactLog",
                newName: "IX_ContactLog_PersonId");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "ContactLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactLog",
                table: "ContactLog",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactLog_TagId",
                table: "ContactLog",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactLog_ContactTags_TagId",
                table: "ContactLog",
                column: "TagId",
                principalTable: "ContactTags",
                principalColumn: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactLog_Persons_PersonId",
                table: "ContactLog",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
