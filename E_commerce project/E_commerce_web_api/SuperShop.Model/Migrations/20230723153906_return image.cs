using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperShop.Model.Migrations
{
    /// <inheritdoc />
    public partial class returnimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "Product",
                newName: "ProductImageId");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "Category",
                newName: "MotherCategoryId");

            migrationBuilder.AddColumn<string>(
                name: "RiderNo",
                table: "RiderInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Return",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReturnReson",
                table: "Return",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Return",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProviderNo",
                table: "ProviderInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductRatingReson",
                table: "ProductRating",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Customer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReturnImage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnImage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductImageId",
                table: "Product",
                column: "ProductImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AddToCart_CustomerId",
                table: "AddToCart",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddToCart_Customer_CustomerId",
                table: "AddToCart",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductImage_ProductImageId",
                table: "Product",
                column: "ProductImageId",
                principalTable: "ProductImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddToCart_Customer_CustomerId",
                table: "AddToCart");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductImage_ProductImageId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "ReturnImage");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductImageId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_AddToCart_CustomerId",
                table: "AddToCart");

            migrationBuilder.DropColumn(
                name: "RiderNo",
                table: "RiderInfo");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Return");

            migrationBuilder.DropColumn(
                name: "ReturnReson",
                table: "Return");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Return");

            migrationBuilder.DropColumn(
                name: "ProviderNo",
                table: "ProviderInfo");

            migrationBuilder.DropColumn(
                name: "ProductRatingReson",
                table: "ProductRating");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "ProductImageId",
                table: "Product",
                newName: "SubCategoryId");

            migrationBuilder.RenameColumn(
                name: "MotherCategoryId",
                table: "Category",
                newName: "SubCategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
