using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLyah_Boutique_System.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderPaymentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Order",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_payment_id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "payment_id",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "Orders",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_client_id",
                table: "Orders",
                newName: "IX_Orders_ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_order_id",
                table: "Payments",
                column: "order_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_order_id",
                table: "Payments",
                column: "order_id",
                principalTable: "Orders",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_order_id",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_order_id",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Orders",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                newName: "IX_Orders_client_id");

            migrationBuilder.AddColumn<int>(
                name: "payment_id",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_payment_id",
                table: "Orders",
                column: "payment_id",
                unique: true,
                filter: "[payment_id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Order",
                table: "Orders",
                column: "payment_id",
                principalTable: "Payments",
                principalColumn: "payment_id");
        }
    }
}
