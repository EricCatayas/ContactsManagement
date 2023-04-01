using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StocksExchange_Removal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Countries_CountryId",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "BuyOrders");

            migrationBuilder.DropTable(
                name: "SellOrders");

            migrationBuilder.AlterColumn<Guid>(
                name: "CountryId",
                table: "Persons",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Countries_CountryId",
                table: "Persons",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Countries_CountryId",
                table: "Persons");

            migrationBuilder.AlterColumn<Guid>(
                name: "CountryId",
                table: "Persons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BuyOrders",
                columns: table => new
                {
                    BuyOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockSymbol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyOrders", x => x.BuyOrderId);
                });

            migrationBuilder.CreateTable(
                name: "SellOrders",
                columns: table => new
                {
                    SellOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockSymbol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellOrders", x => x.SellOrderId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Countries_CountryId",
                table: "Persons",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
