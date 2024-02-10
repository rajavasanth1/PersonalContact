using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalContact.Migrations
{
    /// <inheritdoc />
    public partial class UniqueCountryName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryName",
                table: "Countries",
                column: "CountryName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Countries_CountryName",
                table: "Countries");
        }
    }
}
