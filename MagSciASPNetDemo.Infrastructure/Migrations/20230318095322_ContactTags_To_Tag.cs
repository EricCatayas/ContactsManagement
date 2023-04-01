using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ContactTags_To_Tag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactLog_Persons_PersonId",
                table: "ContactLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactTags_Persons_PersonId",
                table: "ContactTags");

            migrationBuilder.DropIndex(
                name: "IX_ContactTags_PersonId",
                table: "ContactTags");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "ContactTags");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Persons",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PersonId",
                table: "ContactLog",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "ContactLog",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "LogTitle",
                table: "ContactLog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "ContactLog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_TagId",
                table: "Persons",
                column: "TagId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_ContactTags_TagId",
                table: "Persons",
                column: "TagId",
                principalTable: "ContactTags",
                principalColumn: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactLog_ContactTags_TagId",
                table: "ContactLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactLog_Persons_PersonId",
                table: "ContactLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_ContactTags_TagId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_TagId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_ContactLog_TagId",
                table: "ContactLog");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "ContactLog");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonId",
                table: "ContactTags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PersonId",
                table: "ContactLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "ContactLog",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogTitle",
                table: "ContactLog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactTags_PersonId",
                table: "ContactTags",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactLog_Persons_PersonId",
                table: "ContactLog",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactTags_Persons_PersonId",
                table: "ContactTags",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
