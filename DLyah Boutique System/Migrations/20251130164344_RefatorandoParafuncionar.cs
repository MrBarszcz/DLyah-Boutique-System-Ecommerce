using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLyah_Boutique_System.Migrations
{
    /// <inheritdoc />
    public partial class RefatorandoParafuncionar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_User",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Client_User",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_order_id",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Categories_CategoriesCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products_ProductsProductId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Colors_ColorsColorId",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Products_ProductsProductId",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Gender",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Products_ProductsProductId",
                table: "ProductSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizesSizeId",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_Payments_order_id",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "SizesSizeId",
                table: "ProductSizes",
                newName: "SizeId");

            migrationBuilder.RenameColumn(
                name: "ProductsProductId",
                table: "ProductSizes",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSizes_SizesSizeId",
                table: "ProductSizes",
                newName: "IX_ProductSizes_SizeId");

            migrationBuilder.RenameColumn(
                name: "ProductsProductId",
                table: "ProductColors",
                newName: "ColorId");

            migrationBuilder.RenameColumn(
                name: "ColorsColorId",
                table: "ProductColors",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductColors_ProductsProductId",
                table: "ProductColors",
                newName: "IX_ProductColors_ColorId");

            migrationBuilder.RenameColumn(
                name: "ProductsProductId",
                table: "ProductCategories",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoriesCategoryId",
                table: "ProductCategories",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_ProductsProductId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_CategoryId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Orders",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                newName: "IX_Orders_client_id");

            migrationBuilder.AddColumn<int>(
                name: "SizeModelSizeId",
                table: "ProductSizes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColorModelColorId",
                table: "ProductColors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryModelCategoryId",
                table: "ProductCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "payment_id",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "client_address",
                table: "Clients",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "client_cep",
                table: "Clients",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "client_city",
                table: "Clients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "client_state",
                table: "Clients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_date_register",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.CreateTable(
                name: "UserPayments",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    payment_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPayments", x => new { x.user_id, x.payment_id });
                    table.ForeignKey(
                        name: "FK_UserPayment_Payment",
                        column: x => x.payment_id,
                        principalTable: "Payments",
                        principalColumn: "payment_id");
                    table.ForeignKey(
                        name: "FK_UserPayment_User",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_SizeModelSizeId",
                table: "ProductSizes",
                column: "SizeModelSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ColorModelColorId",
                table: "ProductColors",
                column: "ColorModelColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryModelCategoryId",
                table: "ProductCategories",
                column: "CategoryModelCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_payment_id",
                table: "Orders",
                column: "payment_id",
                unique: true,
                filter: "[payment_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserPayments_payment_id",
                table: "UserPayments",
                column: "payment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User",
                table: "Address",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserID",
                table: "Clients",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Order",
                table: "Orders",
                column: "payment_id",
                principalTable: "Payments",
                principalColumn: "payment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Categories_CategoryModelCategoryId",
                table: "ProductCategories",
                column: "CategoryModelCategoryId",
                principalTable: "Categories",
                principalColumn: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Product",
                table: "ProductCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColor_Product",
                table: "ProductColors",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Colors_ColorId",
                table: "ProductColors",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "color_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Colors_ColorModelColorId",
                table: "ProductColors",
                column: "ColorModelColorId",
                principalTable: "Colors",
                principalColumn: "color_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Gender",
                table: "Products",
                column: "gender_id",
                principalTable: "Gender",
                principalColumn: "gender_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSize_Product",
                table: "ProductSizes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizeId",
                table: "ProductSizes",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "size_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizeModelSizeId",
                table: "ProductSizes",
                column: "SizeModelSizeId",
                principalTable: "Sizes",
                principalColumn: "size_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_UserID",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Order",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Categories_CategoryModelCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Product",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColor_Product",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Colors_ColorId",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Colors_ColorModelColorId",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Gender",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSize_Product",
                table: "ProductSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizeId",
                table: "ProductSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizeModelSizeId",
                table: "ProductSizes");

            migrationBuilder.DropTable(
                name: "UserPayments");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizes_SizeModelSizeId",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_ColorModelColorId",
                table: "ProductColors");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_CategoryModelCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_Orders_payment_id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SizeModelSizeId",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "ColorModelColorId",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "CategoryModelCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "payment_id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "client_address",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "client_cep",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "client_city",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "client_state",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "ProductSizes",
                newName: "SizesSizeId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductSizes",
                newName: "ProductsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSizes_SizeId",
                table: "ProductSizes",
                newName: "IX_ProductSizes_SizesSizeId");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                table: "ProductColors",
                newName: "ProductsProductId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductColors",
                newName: "ColorsColorId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductColors_ColorId",
                table: "ProductColors",
                newName: "IX_ProductColors_ProductsProductId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ProductCategories",
                newName: "ProductsProductId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductCategories",
                newName: "CategoriesCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_ProductsProductId");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "Orders",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_client_id",
                table: "Orders",
                newName: "IX_Orders_ClientId");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_date_register",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_order_id",
                table: "Payments",
                column: "order_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_User",
                table: "Address",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_User",
                table: "Clients",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_order_id",
                table: "Payments",
                column: "order_id",
                principalTable: "Orders",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Categories_CategoriesCategoryId",
                table: "ProductCategories",
                column: "CategoriesCategoryId",
                principalTable: "Categories",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products_ProductsProductId",
                table: "ProductCategories",
                column: "ProductsProductId",
                principalTable: "Products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Colors_ColorsColorId",
                table: "ProductColors",
                column: "ColorsColorId",
                principalTable: "Colors",
                principalColumn: "color_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Products_ProductsProductId",
                table: "ProductColors",
                column: "ProductsProductId",
                principalTable: "Products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Gender",
                table: "Products",
                column: "gender_id",
                principalTable: "Gender",
                principalColumn: "gender_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Products_ProductsProductId",
                table: "ProductSizes",
                column: "ProductsProductId",
                principalTable: "Products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizesSizeId",
                table: "ProductSizes",
                column: "SizesSizeId",
                principalTable: "Sizes",
                principalColumn: "size_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
