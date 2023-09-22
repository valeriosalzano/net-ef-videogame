using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net_ef_videogame.Migrations
{
    /// <inheritdoc />
    public partial class CreateVideogamesAndSHsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoftwareHouse",
                columns: table => new
                {
                    SoftwareHouseId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    TaxId = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    City = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Country = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareHouse", x => x.SoftwareHouseId);
                });

            migrationBuilder.CreateTable(
                name: "Videogames",
                columns: table => new
                {
                    VideogameId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Overview = table.Column<string>(type: "VARCHAR(2000)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftwareHouseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videogames", x => x.VideogameId);
                    table.ForeignKey(
                        name: "FK_Videogames_SoftwareHouse_SoftwareHouseId",
                        column: x => x.SoftwareHouseId,
                        principalTable: "SoftwareHouse",
                        principalColumn: "SoftwareHouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareHouse_TaxId",
                table: "SoftwareHouse",
                column: "TaxId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videogames_SoftwareHouseId",
                table: "Videogames",
                column: "SoftwareHouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Videogames");

            migrationBuilder.DropTable(
                name: "SoftwareHouse");
        }
    }
}
