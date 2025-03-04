using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectPos.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateAccountsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 23,
                column: "AccountCategoryId",
                value: 13);

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountCategoryId", "AccountType", "Balance", "Code", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 27, 8, 1, 0.00m, 1227, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Goods For sale", false, "Inventory" },
                    { 28, 3, 5, 0.00m, 1228, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Cost Of Goods Sold", false, "Cost Of Goods Sold" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 23,
                column: "AccountCategoryId",
                value: 12);
        }
    }
}
