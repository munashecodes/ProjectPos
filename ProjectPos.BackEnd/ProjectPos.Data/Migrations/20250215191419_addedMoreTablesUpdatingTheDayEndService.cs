using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPos.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedMoreTablesUpdatingTheDayEndService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPostedToJournal",
                table: "SalesOrderItems",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPostedToJournal",
                table: "PurchaceOrderPayments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPostedToJournal",
                table: "Payments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPostedToJournal",
                table: "GoodsReceivedVouchers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPostedToJournal",
                table: "Expenses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Accounts",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FinancialAccountSnapShots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FinancialAccountId = table.Column<int>(type: "int", nullable: true),
                    ClosingBalance = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    SnapShotDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccountSnapShots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialAccountSnapShots_Accounts_FinancialAccountId",
                        column: x => x.FinancialAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccountSnapShots_FinancialAccountId",
                table: "FinancialAccountSnapShots",
                column: "FinancialAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialAccountSnapShots");

            migrationBuilder.DropColumn(
                name: "IsPostedToJournal",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "IsPostedToJournal",
                table: "PurchaceOrderPayments");

            migrationBuilder.DropColumn(
                name: "IsPostedToJournal",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsPostedToJournal",
                table: "GoodsReceivedVouchers");

            migrationBuilder.DropColumn(
                name: "IsPostedToJournal",
                table: "Expenses");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Accounts",
                type: "decimal(12,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");
        }
    }
}
