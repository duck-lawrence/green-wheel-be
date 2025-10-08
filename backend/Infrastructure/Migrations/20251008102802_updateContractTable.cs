using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateContractTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "station_id",
                table: "rental_contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_rental_contracts_station_id",
                table: "rental_contracts",
                column: "station_id");

            migrationBuilder.AddForeignKey(
                name: "FK_rental_contracts_stations_station_id",
                table: "rental_contracts",
                column: "station_id",
                principalTable: "stations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rental_contracts_stations_station_id",
                table: "rental_contracts");

            migrationBuilder.DropIndex(
                name: "IX_rental_contracts_station_id",
                table: "rental_contracts");

            migrationBuilder.DropColumn(
                name: "station_id",
                table: "rental_contracts");
        }
    }
}
