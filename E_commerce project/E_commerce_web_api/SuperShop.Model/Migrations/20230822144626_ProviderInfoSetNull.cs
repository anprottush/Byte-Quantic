using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperShop.Model.Migrations
{
    /// <inheritdoc />
    public partial class ProviderInfoSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ProductImage",
                newName: "SubFolderLocation");

            migrationBuilder.AddColumn<string>(
                name: "BaseUrl",
                table: "ProductImage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ProductImage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FinalUrl",
                table: "ProductImage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProviderAddress_ProviderId",
                table: "ProviderAddress",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderAddress_ProviderInfo_ProviderId",
                table: "ProviderAddress",
                column: "ProviderId",
                principalTable: "ProviderInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderAddress_ProviderInfo_ProviderId",
                table: "ProviderAddress");

            migrationBuilder.DropIndex(
                name: "IX_ProviderAddress_ProviderId",
                table: "ProviderAddress");

            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "BaseUrl",
                table: "ProductImage");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ProductImage");

            migrationBuilder.DropColumn(
                name: "FinalUrl",
                table: "ProductImage");

            migrationBuilder.RenameColumn(
                name: "SubFolderLocation",
                table: "ProductImage",
                newName: "ImageUrl");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                table: "Product",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
