using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BarberAPI.Migrations
{
    public partial class BarberMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barbers",
                columns: table => new
                {
                    Gd = table.Column<Guid>(nullable: false),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    HPrice = table.Column<double>(nullable: false),
                    BPrice = table.Column<double>(nullable: false),
                    HBPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barbers", x => x.Gd);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Barbers");
        }
    }
}
