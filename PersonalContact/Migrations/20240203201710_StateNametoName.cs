using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalContact.Migrations
{
    /// <inheritdoc />
    public partial class StateNametoName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StateName",
                table: "States",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "States",
                newName: "StateName");
        }
    }
}
