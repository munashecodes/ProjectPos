using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPos.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedFieldsInOvertimeRecordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "OvertimeRecords",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "OvertimeRecords",
                type: "decimal(65,30)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "OvertimeRecords");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "OvertimeRecords");
        }
    }
}
