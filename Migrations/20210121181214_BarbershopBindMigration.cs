using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BarberAPI.Migrations
{
    public partial class BarbershopBindMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BarbershopBinds",
                columns: table => new
                {
                    Gd = table.Column<Guid>(nullable: false),
                    BarbershopGd = table.Column<Guid>(nullable: false),
                    BarberGd = table.Column<Guid>(nullable: false),
                    DateJoined = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarbershopBinds", x => x.Gd);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BarbershopBinds");
        }
    }
}
