using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addstationidtotickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tickets_staff",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "fk_tickets_user",
                table: "tickets");

            migrationBuilder.AlterColumn<Guid>(
                name: "requester_id",
                table: "tickets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "station_id",
                table: "tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "idx_tickets_station_id",
                table: "tickets",
                column: "station_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_stations",
                table: "tickets",
                column: "station_id",
                principalTable: "stations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_staffs",
                table: "tickets",
                column: "assignee_id",
                principalTable: "staffs",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_users",
                table: "tickets",
                column: "requester_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_stations_station_id",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "fk_tickets_staffs",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "fk_tickets_users",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "IX_tickets_station_id",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "station_id",
                table: "tickets");

            migrationBuilder.AlterColumn<Guid>(
                name: "requester_id",
                table: "tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_staff",
                table: "tickets",
                column: "assignee_id",
                principalTable: "staffs",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_user",
                table: "tickets",
                column: "requester_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
