using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Remove_TagDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileAttachment");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ContactTags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ContactTags",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileAttachment",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlobUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ContactLogId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
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
                name: "IX_FileAttachment_ContactLogId",
                table: "FileAttachment",
                column: "ContactLogId");
        }
    }
}
