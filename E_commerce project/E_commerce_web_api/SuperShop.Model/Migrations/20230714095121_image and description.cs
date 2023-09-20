using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperShop.Model.Migrations
{
    /// <inheritdoc />
    public partial class imageanddescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetails",
                newName: "OrderQuantity");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryQuantity",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelivery",
                table: "OrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDateTime",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDelivery",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPartialPayment",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ProviderId",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RiderId",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CityAddress",
                table: "CustomerAddress",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "CustomerLevel",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Brand",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Brand",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "Quantity",
                table: "AddToCart",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DeliveryQuantity",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "IsDelivery",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "DeliveryDateTime",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsFullDelivery",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsPartialPayment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "RiderId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CityAddress",
                table: "CustomerAddress");

            migrationBuilder.DropColumn(
                name: "CustomerLevel",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Brand");

            migrationBuilder.RenameColumn(
                name: "OrderQuantity",
                table: "OrderDetails",
                newName: "Quantity");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "AddToCart",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
