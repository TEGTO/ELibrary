using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    last_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "carts",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_carts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    last_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    address = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    phone = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    user_id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "publishers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_publishers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    order_amount = table.Column<int>(type: "integer", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    delivery_address = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    delivery_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    order_status = table.Column<int>(type: "integer", nullable: false),
                    payment_method = table.Column<int>(type: "integer", nullable: false),
                    delivery_method = table.Column<int>(type: "integer", nullable: false),
                    client_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stock_book_orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    total_change_amount = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    client_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stock_book_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_stock_book_orders_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    publication_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    cover_type = table.Column<int>(type: "integer", nullable: false),
                    page_amount = table.Column<int>(type: "integer", nullable: false),
                    stock_amount = table.Column<int>(type: "integer", nullable: false),
                    cover_img_url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    genre_id = table.Column<int>(type: "integer", nullable: false),
                    publisher_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_books", x => x.id);
                    table.ForeignKey(
                        name: "fk_books_authors_author_id",
                        column: x => x.author_id,
                        principalTable: "authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_books_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_books_publishers_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "publishers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart_books",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    book_amount = table.Column<int>(type: "integer", nullable: false),
                    book_id = table.Column<int>(type: "integer", nullable: false),
                    cart_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cart_books", x => x.id);
                    table.ForeignKey(
                        name: "fk_cart_books_books_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cart_books_carts_cart_id",
                        column: x => x.cart_id,
                        principalTable: "carts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_books",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    book_amount = table.Column<int>(type: "integer", nullable: false),
                    book_id = table.Column<int>(type: "integer", nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_books", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_books_books_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_order_books_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stock_book_changes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    book_id = table.Column<int>(type: "integer", nullable: false),
                    change_amount = table.Column<int>(type: "integer", nullable: false),
                    stock_book_order_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stock_book_changes", x => x.id);
                    table.ForeignKey(
                        name: "fk_stock_book_changes_books_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_stock_book_changes_stock_book_orders_stock_book_order_id",
                        column: x => x.stock_book_order_id,
                        principalTable: "stock_book_orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_books_author_id",
                table: "books",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_books_genre_id",
                table: "books",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "ix_books_publisher_id",
                table: "books",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "ix_cart_books_book_id",
                table: "cart_books",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "ix_cart_books_cart_id",
                table: "cart_books",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "ix_clients_user_id",
                table: "clients",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_books_book_id",
                table: "order_books",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_books_order_id",
                table: "order_books",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_client_id",
                table: "orders",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_stock_book_changes_book_id",
                table: "stock_book_changes",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "ix_stock_book_changes_stock_book_order_id",
                table: "stock_book_changes",
                column: "stock_book_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_stock_book_orders_client_id",
                table: "stock_book_orders",
                column: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_books");

            migrationBuilder.DropTable(
                name: "order_books");

            migrationBuilder.DropTable(
                name: "stock_book_changes");

            migrationBuilder.DropTable(
                name: "carts");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "stock_book_orders");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "publishers");

            migrationBuilder.DropTable(
                name: "clients");
        }
    }
}
