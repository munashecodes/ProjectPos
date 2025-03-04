using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPos.Data.Migrations
{
    /// <inheritdoc />
    public partial class isReturnedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isReturned",
                table: "SalesOrderItems",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isReturned",
                table: "SalesOrderItems");
        }
    }
}
