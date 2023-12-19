using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POSWebsite.Migrations
{
    /// <inheritdoc />
    public partial class ModifyProductAndBranchRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_BranchStore_BranchStoreId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_BranchStoreId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "BranchStoreId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Product");

            migrationBuilder.CreateTable(
                name: "ProductBranch",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBranch", x => new { x.BranchId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductBranch_BranchStore_BranchId",
                        column: x => x.BranchId,
                        principalTable: "BranchStore",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductBranch_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBranch_ProductId",
                table: "ProductBranch",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBranch");

            migrationBuilder.AddColumn<int>(
                name: "BranchStoreId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_BranchStoreId",
                table: "Product",
                column: "BranchStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_BranchStore_BranchStoreId",
                table: "Product",
                column: "BranchStoreId",
                principalTable: "BranchStore",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
