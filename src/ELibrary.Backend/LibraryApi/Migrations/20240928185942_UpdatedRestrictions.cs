using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRestrictions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderBooks_Books_BookId",
                table: "OrderBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBookChanges_Books_BookId",
                table: "StockBookChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBookOrders_Clients_ClientId",
                table: "StockBookOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBooks_Books_BookId",
                table: "OrderBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBookChanges_Books_BookId",
                table: "StockBookChanges",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBookOrders_Clients_ClientId",
                table: "StockBookOrders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderBooks_Books_BookId",
                table: "OrderBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBookChanges_Books_BookId",
                table: "StockBookChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBookOrders_Clients_ClientId",
                table: "StockBookOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBooks_Books_BookId",
                table: "OrderBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBookChanges_Books_BookId",
                table: "StockBookChanges",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBookOrders_Clients_ClientId",
                table: "StockBookOrders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
