using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelloApi.Migrations
{
    /// <inheritdoc />
    public partial class TripImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_User_UserId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_UserId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Photos");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Trip",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileType",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IdentityId",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_User_PhotoId",
                table: "User",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_ImageId",
                table: "Trip",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Photos_ImageId",
                table: "Trip",
                column: "ImageId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Photos_PhotoId",
                table: "User",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Photos_ImageId",
                table: "Trip");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Photos_PhotoId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PhotoId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Trip_ImageId",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Trip",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Photos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_User_UserId",
                table: "Photos",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
