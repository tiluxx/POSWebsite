using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POSWebsite.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCreationDateForCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActualPrice",
                table: "OrderDetail",
                newName: "ActualUnitPrice");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Customer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "ActualUnitPrice",
                table: "OrderDetail",
                newName: "ActualPrice");
        }
    }
}
