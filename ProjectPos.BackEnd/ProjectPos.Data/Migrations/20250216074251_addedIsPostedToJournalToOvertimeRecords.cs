using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPos.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedIsPostedToJournalToOvertimeRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPostedToJournal",
                table: "OvertimeRecords",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPostedToJournal",
                table: "OvertimeRecords");
        }
    }
}
