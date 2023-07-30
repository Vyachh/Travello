using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelloApi.Migrations
{
    /// <inheritdoc />
    public partial class TripPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Trip",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Trip");
        }
    }
}
