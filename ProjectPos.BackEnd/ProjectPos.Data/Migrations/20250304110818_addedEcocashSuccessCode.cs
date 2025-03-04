using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPos.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedEcocashSuccessCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EcocashSuccessCode",
                table: "Payments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EcocashSuccessCode",
                table: "Payments");
        }
    }
}
