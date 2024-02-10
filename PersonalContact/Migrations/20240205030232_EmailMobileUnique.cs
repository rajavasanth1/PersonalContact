using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalContact.Migrations
{
    /// <inheritdoc />
    public partial class EmailMobileUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contact_Email",
                table: "Contact",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_Mobile",
                table: "Contact",
                column: "Mobile",
                unique: true,
                filter: "[Mobile] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contact_Email",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_Mobile",
                table: "Contact");
        }
    }
}
