using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uni_Movie.Migrations
{
    /// <inheritdoc />
    public partial class addUserForiegnKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "VisitedGenres",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitedGenres_userId",
                table: "VisitedGenres",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitedGenres_AspNetUsers_userId",
                table: "VisitedGenres",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitedGenres_AspNetUsers_userId",
                table: "VisitedGenres");

            migrationBuilder.DropIndex(
                name: "IX_VisitedGenres_userId",
                table: "VisitedGenres");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "VisitedGenres");
        }
    }
}
