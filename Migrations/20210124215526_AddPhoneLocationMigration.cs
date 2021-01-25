using Microsoft.EntityFrameworkCore.Migrations;

namespace BarberAPI.Migrations
{
    public partial class AddPhoneLocationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Barbershops",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Barbershops",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Barbers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Barbershops");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Barbershops");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Barbers");
        }
    }
}
