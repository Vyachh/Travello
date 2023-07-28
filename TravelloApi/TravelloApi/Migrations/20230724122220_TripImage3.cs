using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelloApi.Migrations
{
    /// <inheritdoc />
    public partial class TripImage3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Trip",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Trip",
                newName: "ImageName");
        }
    }
}
