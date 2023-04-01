using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename_to_IdConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactGroupPerson_ContactGroups_ContactGroupsId",
                table: "ContactGroupPerson");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ContactTags",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ContactGroups",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "ContactGroupsId",
                table: "ContactGroupPerson",
                newName: "ContactGroupsGroupId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Companies",
                newName: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactGroupPerson_ContactGroups_ContactGroupsGroupId",
                table: "ContactGroupPerson",
                column: "ContactGroupsGroupId",
                principalTable: "ContactGroups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactGroupPerson_ContactGroups_ContactGroupsGroupId",
                table: "ContactGroupPerson");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "ContactTags",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "ContactGroups",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ContactGroupsGroupId",
                table: "ContactGroupPerson",
                newName: "ContactGroupsId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Companies",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactGroupPerson_ContactGroups_ContactGroupsId",
                table: "ContactGroupPerson",
                column: "ContactGroupsId",
                principalTable: "ContactGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
