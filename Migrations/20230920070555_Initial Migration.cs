using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking_Room.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "RoomTypes");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
