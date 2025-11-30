using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLyah_Boutique_System.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name_complete = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    user_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    user_date_register = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    user_email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    banner_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    banner_title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    banner_description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    banner_path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    banner_link_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    banner_is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.banner_id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    color_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hex_color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.color_id);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    gender_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.gender_id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<int>(type: "int", nullable: false),
                    payment_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    payment_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    payment_value_paid = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    payment_date = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.payment_id);
                    table.CheckConstraint("CK_PaymentMethod", "[payment_method] IN ('Dinheiro', 'PIX', 'Boleto', 'Cartão de Crédito')");
                    table.CheckConstraint("CK_PaymentStatus", "[payment_status] IN ('Estornado', 'Recusado', 'Aprovado', 'Aguardando')");
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    size_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    size = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.size_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    address_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    address_number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    address_complement = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address_neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address_city = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address_state = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    address_cep = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    address_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "ENTREGA")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.address_id);
                    table.ForeignKey(
                        name: "FK_Addresses_User",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    client_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_cpf = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    client_phonenumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    client_date_birth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    client_date_register = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    client_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "ativo"),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.client_id);
                    table.CheckConstraint("CK_ClientStatus", "[client_status] IN ('suspenso', 'inativo', 'ativo')");
                    table.ForeignKey(
                        name: "FK_Client_User",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileImages",
                columns: table => new
                {
                    user_image_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    user_image_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileImages", x => x.user_image_id);
                    table.ForeignKey(
                        name: "FK_UserProfileImage_User",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BannersPlacements",
                columns: table => new
                {
                    banner_placement_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    banner_placement_pagename = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    banner_placement_position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    banner_placement_order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    banner_placement_is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    banner_id = table.Column<int>(type: "int", nullable: false),
                    display_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    layout_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannersPlacements", x => x.banner_placement_id);
                    table.ForeignKey(
                        name: "FK_BannerPlacements_Banners",
                        column: x => x.banner_id,
                        principalTable: "Banners",
                        principalColumn: "banner_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    product_description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    product_price = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    gender_id = table.Column<int>(type: "int", nullable: false),
                    product_quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_Product_Gender",
                        column: x => x.gender_id,
                        principalTable: "Gender",
                        principalColumn: "gender_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_id = table.Column<int>(type: "int", nullable: false),
                    date_order = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    order_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    order_value_total = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    payment_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.order_id);
                    table.CheckConstraint("CK_OrderStatus", "[order_status] IN ('cancelado', 'enviado', 'pago', 'pendente')");
                    table.ForeignKey(
                        name: "FK_Order_Client",
                        column: x => x.client_id,
                        principalTable: "Clients",
                        principalColumn: "client_id");
                    table.ForeignKey(
                        name: "FK_Payment_Order",
                        column: x => x.payment_id,
                        principalTable: "Payments",
                        principalColumn: "payment_id");
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    CategoriesCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => new { x.CategoriesCategoryId, x.ProductsProductId });
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoriesCategoryId",
                        column: x => x.CategoriesCategoryId,
                        principalTable: "Categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductColors",
                columns: table => new
                {
                    ColorsColorId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColors", x => new { x.ColorsColorId, x.ProductsProductId });
                    table.ForeignKey(
                        name: "FK_ProductColors_Colors_ColorsColorId",
                        column: x => x.ColorsColorId,
                        principalTable: "Colors",
                        principalColumn: "color_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductColors_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    product_image_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    product_image_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    image_order = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.product_image_id);
                    table.ForeignKey(
                        name: "FK_ProductImage_Product",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSizes",
                columns: table => new
                {
                    ProductsProductId = table.Column<int>(type: "int", nullable: false),
                    SizesSizeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSizes", x => new { x.ProductsProductId, x.SizesSizeId });
                    table.ForeignKey(
                        name: "FK_ProductSizes_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSizes_Sizes_SizesSizeId",
                        column: x => x.SizesSizeId,
                        principalTable: "Sizes",
                        principalColumn: "size_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockProducts",
                columns: table => new
                {
                    stock_product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    color_id = table.Column<int>(type: "int", nullable: false),
                    size_id = table.Column<int>(type: "int", nullable: false),
                    quantity_stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockProducts", x => x.stock_product_id);
                    table.ForeignKey(
                        name: "FK_StockItem_Color",
                        column: x => x.color_id,
                        principalTable: "Colors",
                        principalColumn: "color_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockItem_Product",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockItem_Size",
                        column: x => x.size_id,
                        principalTable: "Sizes",
                        principalColumn: "size_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price_per_unit = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    total_value = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => new { x.order_id, x.product_id });
                    table.ForeignKey(
                        name: "FK_OrderItem_Order",
                        column: x => x.order_id,
                        principalTable: "Orders",
                        principalColumn: "order_id");
                    table.ForeignKey(
                        name: "FK_OrderItem_Product",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "product_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_user_id",
                table: "Address",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BannersPlacements_banner_id",
                table: "BannersPlacements",
                column: "banner_id");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_client_cpf",
                table: "Clients",
                column: "client_cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_user_id",
                table: "Clients",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_color",
                table: "Colors",
                column: "color",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_hex_color",
                table: "Colors",
                column: "hex_color",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gender_gender",
                table: "Gender",
                column: "gender",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_product_id",
                table: "OrderItems",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_client_id",
                table: "Orders",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_payment_id",
                table: "Orders",
                column: "payment_id",
                unique: true,
                filter: "[payment_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductsProductId",
                table: "ProductCategories",
                column: "ProductsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ProductsProductId",
                table: "ProductColors",
                column: "ProductsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_product_id",
                table: "ProductImages",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_gender_id",
                table: "Products",
                column: "gender_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_SizesSizeId",
                table: "ProductSizes",
                column: "SizesSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_size",
                table: "Sizes",
                column: "size",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_color_id",
                table: "StockProducts",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_size_id",
                table: "StockProducts",
                column: "size_id");

            migrationBuilder.CreateIndex(
                name: "UQ_StockItem_ProductColorSize",
                table: "StockProducts",
                columns: new[] { "product_id", "color_id", "size_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileImages_user_id",
                table: "UserProfileImages",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BannersPlacements");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductColors");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductSizes");

            migrationBuilder.DropTable(
                name: "StockProducts");

            migrationBuilder.DropTable(
                name: "UserProfileImages");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
