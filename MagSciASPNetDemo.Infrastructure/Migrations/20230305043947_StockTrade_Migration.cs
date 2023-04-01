using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StockTrade_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyOrders",
                columns: table => new
                {
                    BuyOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockSymbol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true)
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
                    StockSymbol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellOrders", x => x.SellOrderId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyOrders");

            migrationBuilder.DropTable(
                name: "SellOrders");
        }
    }
}
