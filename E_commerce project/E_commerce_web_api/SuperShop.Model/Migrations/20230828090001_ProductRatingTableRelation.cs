using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperShop.Model.Migrations
{
    /// <inheritdoc />
    public partial class ProductRatingTableRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressTypeAsString",
                table: "ProviderAddress");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "RiderInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRating_CustomerId",
                table: "ProductRating",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRating_ProductId",
                table: "ProductRating",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRating_Customer_CustomerId",
                table: "ProductRating",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRating_Product_ProductId",
                table: "ProductRating",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRating_Customer_CustomerId",
                table: "ProductRating");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRating_Product_ProductId",
                table: "ProductRating");

            migrationBuilder.DropIndex(
                name: "IX_ProductRating_CustomerId",
                table: "ProductRating");

            migrationBuilder.DropIndex(
                name: "IX_ProductRating_ProductId",
                table: "ProductRating");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "RiderInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressTypeAsString",
                table: "ProviderAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
