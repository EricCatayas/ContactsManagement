using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ContactsManagementExpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Persons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Persons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber1",
                table: "Persons",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber2",
                table: "Persons",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Persons",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileBlobUrl",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                table: "Countries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyProfileBlobUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ContactNumber2 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactLog",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LogTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactLog", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_ContactLog_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TagColor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactTags_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContactGroupPerson",
                columns: table => new
                {
                    ContactGroupsId = table.Column<int>(type: "int", nullable: false),
                    PersonsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactGroupPerson", x => new { x.ContactGroupsId, x.PersonsId });
                    table.ForeignKey(
                        name: "FK_ContactGroupPerson_ContactGroups_ContactGroupsId",
                        column: x => x.ContactGroupsId,
                        principalTable: "ContactGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactGroupPerson_Persons_PersonsId",
                        column: x => x.PersonsId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileAttachment",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BlobUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ContactLogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAttachment", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_FileAttachment_ContactLog_ContactLogId",
                        column: x => x.ContactLogId,
                        principalTable: "ContactLog",
                        principalColumn: "LogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CompanyId",
                table: "Persons",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactGroupPerson_PersonsId",
                table: "ContactGroupPerson",
                column: "PersonsId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactLog_PersonId",
                table: "ContactLog",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactTags_PersonId",
                table: "ContactTags",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_ContactLogId",
                table: "FileAttachment",
                column: "ContactLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Companies_CompanyId",
                table: "Persons",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Companies_CompanyId",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "ContactGroupPerson");

            migrationBuilder.DropTable(
                name: "ContactTags");

            migrationBuilder.DropTable(
                name: "FileAttachment");

            migrationBuilder.DropTable(
                name: "ContactGroups");

            migrationBuilder.DropTable(
                name: "ContactLog");

            migrationBuilder.DropIndex(
                name: "IX_Persons_CompanyId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "ContactNumber1",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "ContactNumber2",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "ProfileBlobUrl",
                table: "Persons");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Persons",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                table: "Countries",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
