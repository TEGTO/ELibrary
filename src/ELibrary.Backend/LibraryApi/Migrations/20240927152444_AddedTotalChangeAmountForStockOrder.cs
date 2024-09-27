using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedTotalChangeAmountForStockOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockBookChanges_StockBookOrders_StockBookOrderId",
                table: "StockBookChanges");

            migrationBuilder.AddColumn<int>(
                name: "TotalChangeAmount",
                table: "StockBookOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StockBookOrderId",
                table: "StockBookChanges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_StockBookChanges_StockBookOrders_StockBookOrderId",
                table: "StockBookChanges",
                column: "StockBookOrderId",
                principalTable: "StockBookOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockBookChanges_StockBookOrders_StockBookOrderId",
                table: "StockBookChanges");

            migrationBuilder.DropColumn(
                name: "TotalChangeAmount",
                table: "StockBookOrders");

            migrationBuilder.AlterColumn<int>(
                name: "StockBookOrderId",
                table: "StockBookChanges",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBookChanges_StockBookOrders_StockBookOrderId",
                table: "StockBookChanges",
                column: "StockBookOrderId",
                principalTable: "StockBookOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
