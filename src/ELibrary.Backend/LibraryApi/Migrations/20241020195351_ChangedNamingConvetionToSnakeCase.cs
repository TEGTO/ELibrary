using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNamingConvetionToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Genres_GenreId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_CartBooks_Books_BookId",
                table: "CartBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_CartBooks_Carts_CartId",
                table: "CartBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderBooks_Books_BookId",
                table: "OrderBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderBooks_Orders_OrderId",
                table: "OrderBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBookChanges_Books_BookId",
                table: "StockBookChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBookChanges_StockBookOrders_StockBookOrderId",
                table: "StockBookChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBookOrders_Clients_ClientId",
                table: "StockBookOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publishers",
                table: "Publishers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authors",
                table: "Authors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockBookOrders",
                table: "StockBookOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockBookChanges",
                table: "StockBookChanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderBooks",
                table: "OrderBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartBooks",
                table: "CartBooks");

            migrationBuilder.RenameTable(
                name: "Publishers",
                newName: "publishers");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "orders");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "genres");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "clients");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "carts");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "books");

            migrationBuilder.RenameTable(
                name: "Authors",
                newName: "authors");

            migrationBuilder.RenameTable(
                name: "StockBookOrders",
                newName: "stock_book_orders");

            migrationBuilder.RenameTable(
                name: "StockBookChanges",
                newName: "stock_book_changes");

            migrationBuilder.RenameTable(
                name: "OrderBooks",
                newName: "order_books");

            migrationBuilder.RenameTable(
                name: "CartBooks",
                newName: "cart_books");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "publishers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "publishers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "orders",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "orders",
                newName: "total_price");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "orders",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "orders",
                newName: "order_status");

            migrationBuilder.RenameColumn(
                name: "OrderAmount",
                table: "orders",
                newName: "order_amount");

            migrationBuilder.RenameColumn(
                name: "DeliveryTime",
                table: "orders",
                newName: "delivery_time");

            migrationBuilder.RenameColumn(
                name: "DeliveryMethod",
                table: "orders",
                newName: "delivery_method");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddress",
                table: "orders",
                newName: "delivery_address");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "orders",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "orders",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ClientId",
                table: "orders",
                newName: "ix_orders_client_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "genres",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "genres",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "clients",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "clients",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "clients",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "clients",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "clients",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "clients",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "clients",
                newName: "middle_name");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "clients",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "clients",
                newName: "date_of_birth");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_UserId",
                table: "clients",
                newName: "ix_clients_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "carts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "carts",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "books",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "books",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "books",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StockAmount",
                table: "books",
                newName: "stock_amount");

            migrationBuilder.RenameColumn(
                name: "PublisherId",
                table: "books",
                newName: "publisher_id");

            migrationBuilder.RenameColumn(
                name: "PublicationDate",
                table: "books",
                newName: "publication_date");

            migrationBuilder.RenameColumn(
                name: "PageAmount",
                table: "books",
                newName: "page_amount");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "books",
                newName: "genre_id");

            migrationBuilder.RenameColumn(
                name: "CoverType",
                table: "books",
                newName: "cover_type");

            migrationBuilder.RenameColumn(
                name: "CoverImgUrl",
                table: "books",
                newName: "cover_img_url");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "books",
                newName: "author_id");

            migrationBuilder.RenameIndex(
                name: "IX_Books_PublisherId",
                table: "books",
                newName: "ix_books_publisher_id");

            migrationBuilder.RenameIndex(
                name: "IX_Books_GenreId",
                table: "books",
                newName: "ix_books_genre_id");

            migrationBuilder.RenameIndex(
                name: "IX_Books_AuthorId",
                table: "books",
                newName: "ix_books_author_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "authors",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "authors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "authors",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "authors",
                newName: "date_of_birth");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "stock_book_orders",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "stock_book_orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "stock_book_orders",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalChangeAmount",
                table: "stock_book_orders",
                newName: "total_change_amount");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "stock_book_orders",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "stock_book_orders",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_StockBookOrders_ClientId",
                table: "stock_book_orders",
                newName: "ix_stock_book_orders_client_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "stock_book_changes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StockBookOrderId",
                table: "stock_book_changes",
                newName: "stock_book_order_id");

            migrationBuilder.RenameColumn(
                name: "ChangeAmount",
                table: "stock_book_changes",
                newName: "change_amount");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "stock_book_changes",
                newName: "book_id");

            migrationBuilder.RenameIndex(
                name: "IX_StockBookChanges_StockBookOrderId",
                table: "stock_book_changes",
                newName: "ix_stock_book_changes_stock_book_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_StockBookChanges_BookId",
                table: "stock_book_changes",
                newName: "ix_stock_book_changes_book_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "order_books",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "order_books",
                newName: "order_id");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "order_books",
                newName: "book_id");

            migrationBuilder.RenameColumn(
                name: "BookAmount",
                table: "order_books",
                newName: "book_amount");

            migrationBuilder.RenameIndex(
                name: "IX_OrderBooks_OrderId",
                table: "order_books",
                newName: "ix_order_books_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderBooks_BookId",
                table: "order_books",
                newName: "ix_order_books_book_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "cart_books",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "cart_books",
                newName: "cart_id");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "cart_books",
                newName: "book_id");

            migrationBuilder.RenameColumn(
                name: "BookAmount",
                table: "cart_books",
                newName: "book_amount");

            migrationBuilder.RenameIndex(
                name: "IX_CartBooks_CartId",
                table: "cart_books",
                newName: "ix_cart_books_cart_id");

            migrationBuilder.RenameIndex(
                name: "IX_CartBooks_BookId",
                table: "cart_books",
                newName: "ix_cart_books_book_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_publishers",
                table: "publishers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_orders",
                table: "orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_genres",
                table: "genres",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_clients",
                table: "clients",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_carts",
                table: "carts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_books",
                table: "books",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_authors",
                table: "authors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_stock_book_orders",
                table: "stock_book_orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_stock_book_changes",
                table: "stock_book_changes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order_books",
                table: "order_books",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_cart_books",
                table: "cart_books",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_books_authors_author_id",
                table: "books",
                column: "author_id",
                principalTable: "authors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_books_genres_genre_id",
                table: "books",
                column: "genre_id",
                principalTable: "genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_books_publishers_publisher_id",
                table: "books",
                column: "publisher_id",
                principalTable: "publishers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cart_books_books_book_id",
                table: "cart_books",
                column: "book_id",
                principalTable: "books",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cart_books_carts_cart_id",
                table: "cart_books",
                column: "cart_id",
                principalTable: "carts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_order_books_books_book_id",
                table: "order_books",
                column: "book_id",
                principalTable: "books",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_order_books_orders_order_id",
                table: "order_books",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_orders_clients_client_id",
                table: "orders",
                column: "client_id",
                principalTable: "clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_stock_book_changes_books_book_id",
                table: "stock_book_changes",
                column: "book_id",
                principalTable: "books",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_stock_book_changes_stock_book_orders_stock_book_order_id",
                table: "stock_book_changes",
                column: "stock_book_order_id",
                principalTable: "stock_book_orders",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_stock_book_orders_clients_client_id",
                table: "stock_book_orders",
                column: "client_id",
                principalTable: "clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_books_authors_author_id",
                table: "books");

            migrationBuilder.DropForeignKey(
                name: "fk_books_genres_genre_id",
                table: "books");

            migrationBuilder.DropForeignKey(
                name: "fk_books_publishers_publisher_id",
                table: "books");

            migrationBuilder.DropForeignKey(
                name: "fk_cart_books_books_book_id",
                table: "cart_books");

            migrationBuilder.DropForeignKey(
                name: "fk_cart_books_carts_cart_id",
                table: "cart_books");

            migrationBuilder.DropForeignKey(
                name: "fk_order_books_books_book_id",
                table: "order_books");

            migrationBuilder.DropForeignKey(
                name: "fk_order_books_orders_order_id",
                table: "order_books");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_clients_client_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "fk_stock_book_changes_books_book_id",
                table: "stock_book_changes");

            migrationBuilder.DropForeignKey(
                name: "fk_stock_book_changes_stock_book_orders_stock_book_order_id",
                table: "stock_book_changes");

            migrationBuilder.DropForeignKey(
                name: "fk_stock_book_orders_clients_client_id",
                table: "stock_book_orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_publishers",
                table: "publishers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_orders",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_genres",
                table: "genres");

            migrationBuilder.DropPrimaryKey(
                name: "pk_clients",
                table: "clients");

            migrationBuilder.DropPrimaryKey(
                name: "pk_carts",
                table: "carts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_books",
                table: "books");

            migrationBuilder.DropPrimaryKey(
                name: "pk_authors",
                table: "authors");

            migrationBuilder.DropPrimaryKey(
                name: "pk_stock_book_orders",
                table: "stock_book_orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_stock_book_changes",
                table: "stock_book_changes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_order_books",
                table: "order_books");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cart_books",
                table: "cart_books");

            migrationBuilder.RenameTable(
                name: "publishers",
                newName: "Publishers");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "genres",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "clients",
                newName: "Clients");

            migrationBuilder.RenameTable(
                name: "carts",
                newName: "Carts");

            migrationBuilder.RenameTable(
                name: "books",
                newName: "Books");

            migrationBuilder.RenameTable(
                name: "authors",
                newName: "Authors");

            migrationBuilder.RenameTable(
                name: "stock_book_orders",
                newName: "StockBookOrders");

            migrationBuilder.RenameTable(
                name: "stock_book_changes",
                newName: "StockBookChanges");

            migrationBuilder.RenameTable(
                name: "order_books",
                newName: "OrderBooks");

            migrationBuilder.RenameTable(
                name: "cart_books",
                newName: "CartBooks");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Publishers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Publishers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Orders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_price",
                table: "Orders",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "Orders",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "order_status",
                table: "Orders",
                newName: "OrderStatus");

            migrationBuilder.RenameColumn(
                name: "order_amount",
                table: "Orders",
                newName: "OrderAmount");

            migrationBuilder.RenameColumn(
                name: "delivery_time",
                table: "Orders",
                newName: "DeliveryTime");

            migrationBuilder.RenameColumn(
                name: "delivery_method",
                table: "Orders",
                newName: "DeliveryMethod");

            migrationBuilder.RenameColumn(
                name: "delivery_address",
                table: "Orders",
                newName: "DeliveryAddress");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Orders",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "Orders",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "ix_orders_client_id",
                table: "Orders",
                newName: "IX_Orders_ClientId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Genres",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Genres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Clients",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Clients",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Clients",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Clients",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Clients",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Clients",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "middle_name",
                table: "Clients",
                newName: "MiddleName");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Clients",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "date_of_birth",
                table: "Clients",
                newName: "DateOfBirth");

            migrationBuilder.RenameIndex(
                name: "ix_clients_user_id",
                table: "Clients",
                newName: "IX_Clients_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Carts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Carts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Books",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Books",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Books",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "stock_amount",
                table: "Books",
                newName: "StockAmount");

            migrationBuilder.RenameColumn(
                name: "publisher_id",
                table: "Books",
                newName: "PublisherId");

            migrationBuilder.RenameColumn(
                name: "publication_date",
                table: "Books",
                newName: "PublicationDate");

            migrationBuilder.RenameColumn(
                name: "page_amount",
                table: "Books",
                newName: "PageAmount");

            migrationBuilder.RenameColumn(
                name: "genre_id",
                table: "Books",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "cover_type",
                table: "Books",
                newName: "CoverType");

            migrationBuilder.RenameColumn(
                name: "cover_img_url",
                table: "Books",
                newName: "CoverImgUrl");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "Books",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "ix_books_publisher_id",
                table: "Books",
                newName: "IX_Books_PublisherId");

            migrationBuilder.RenameIndex(
                name: "ix_books_genre_id",
                table: "Books",
                newName: "IX_Books_GenreId");

            migrationBuilder.RenameIndex(
                name: "ix_books_author_id",
                table: "Books",
                newName: "IX_Books_AuthorId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Authors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Authors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Authors",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "date_of_birth",
                table: "Authors",
                newName: "DateOfBirth");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "StockBookOrders",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StockBookOrders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "StockBookOrders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_change_amount",
                table: "StockBookOrders",
                newName: "TotalChangeAmount");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "StockBookOrders",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "StockBookOrders",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "ix_stock_book_orders_client_id",
                table: "StockBookOrders",
                newName: "IX_StockBookOrders_ClientId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StockBookChanges",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "stock_book_order_id",
                table: "StockBookChanges",
                newName: "StockBookOrderId");

            migrationBuilder.RenameColumn(
                name: "change_amount",
                table: "StockBookChanges",
                newName: "ChangeAmount");

            migrationBuilder.RenameColumn(
                name: "book_id",
                table: "StockBookChanges",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "ix_stock_book_changes_stock_book_order_id",
                table: "StockBookChanges",
                newName: "IX_StockBookChanges_StockBookOrderId");

            migrationBuilder.RenameIndex(
                name: "ix_stock_book_changes_book_id",
                table: "StockBookChanges",
                newName: "IX_StockBookChanges_BookId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "OrderBooks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "order_id",
                table: "OrderBooks",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "book_id",
                table: "OrderBooks",
                newName: "BookId");

            migrationBuilder.RenameColumn(
                name: "book_amount",
                table: "OrderBooks",
                newName: "BookAmount");

            migrationBuilder.RenameIndex(
                name: "ix_order_books_order_id",
                table: "OrderBooks",
                newName: "IX_OrderBooks_OrderId");

            migrationBuilder.RenameIndex(
                name: "ix_order_books_book_id",
                table: "OrderBooks",
                newName: "IX_OrderBooks_BookId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CartBooks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "cart_id",
                table: "CartBooks",
                newName: "CartId");

            migrationBuilder.RenameColumn(
                name: "book_id",
                table: "CartBooks",
                newName: "BookId");

            migrationBuilder.RenameColumn(
                name: "book_amount",
                table: "CartBooks",
                newName: "BookAmount");

            migrationBuilder.RenameIndex(
                name: "ix_cart_books_cart_id",
                table: "CartBooks",
                newName: "IX_CartBooks_CartId");

            migrationBuilder.RenameIndex(
                name: "ix_cart_books_book_id",
                table: "CartBooks",
                newName: "IX_CartBooks_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publishers",
                table: "Publishers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authors",
                table: "Authors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockBookOrders",
                table: "StockBookOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockBookChanges",
                table: "StockBookChanges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderBooks",
                table: "OrderBooks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartBooks",
                table: "CartBooks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Genres_GenreId",
                table: "Books",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartBooks_Books_BookId",
                table: "CartBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartBooks_Carts_CartId",
                table: "CartBooks",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBooks_Books_BookId",
                table: "OrderBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBooks_Orders_OrderId",
                table: "OrderBooks",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBookChanges_Books_BookId",
                table: "StockBookChanges",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBookChanges_StockBookOrders_StockBookOrderId",
                table: "StockBookChanges",
                column: "StockBookOrderId",
                principalTable: "StockBookOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockBookOrders_Clients_ClientId",
                table: "StockBookOrders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
