using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BarberAPI.Migrations
{
    public partial class BarbershopMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Gd = table.Column<Guid>(nullable: false),
                    BarberGd = table.Column<Guid>(nullable: false),
                    ClientGd = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    IsCanceled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Gd);
                });

            migrationBuilder.CreateTable(
                name: "Barbers",
                columns: table => new
                {
                    Gd = table.Column<Guid>(nullable: false),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
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

            migrationBuilder.CreateTable(
                name: "BarbershopOwners",
                columns: table => new
                {
                    Gd = table.Column<Guid>(nullable: false),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarbershopOwners", x => x.Gd);
                });

            migrationBuilder.CreateTable(
                name: "Barbershops",
                columns: table => new
                {
                    Gd = table.Column<Guid>(nullable: false),
                    Shopname = table.Column<string>(nullable: true),
                    OwnerGd = table.Column<Guid>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    DateOpened = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barbershops", x => x.Gd);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Gd = table.Column<Guid>(nullable: false),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Gd);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Barbers");

            migrationBuilder.DropTable(
                name: "BarbershopOwners");

            migrationBuilder.DropTable(
                name: "Barbershops");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
