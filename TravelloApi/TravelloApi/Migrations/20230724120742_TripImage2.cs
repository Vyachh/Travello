using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelloApi.Migrations
{
    /// <inheritdoc />
    public partial class TripImage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Photos_ImageId",
                table: "Trip");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Photos_PhotoId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Trip_ImageId",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Trip");

            migrationBuilder.AlterColumn<int>(
                name: "PhotoId",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Trip",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Photos_PhotoId",
                table: "User",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Photos_PhotoId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Trip");

            migrationBuilder.AlterColumn<int>(
                name: "PhotoId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Trip",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
