using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_indentity_and_driver_license : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "driver_licenses",
                newName: "front_image_url");

            migrationBuilder.RenameColumn(
                name: "image_public_id",
                table: "driver_licenses",
                newName: "front_image_public_id");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "citizen_identities",
                newName: "front_image_url");

            migrationBuilder.RenameColumn(
                name: "image_public_id",
                table: "citizen_identities",
                newName: "front_image_public_id");

            migrationBuilder.AddColumn<string>(
                name: "back_image_public_id",
                table: "driver_licenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "back_image_url",
                table: "driver_licenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "back_image_public_id",
                table: "citizen_identities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "back_image_url",
                table: "citizen_identities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "back_image_public_id",
                table: "driver_licenses");

            migrationBuilder.DropColumn(
                name: "back_image_url",
                table: "driver_licenses");

            migrationBuilder.DropColumn(
                name: "back_image_public_id",
                table: "citizen_identities");

            migrationBuilder.DropColumn(
                name: "back_image_url",
                table: "citizen_identities");

            migrationBuilder.RenameColumn(
                name: "front_image_url",
                table: "driver_licenses",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "front_image_public_id",
                table: "driver_licenses",
                newName: "image_public_id");

            migrationBuilder.RenameColumn(
                name: "front_image_url",
                table: "citizen_identities",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "front_image_public_id",
                table: "citizen_identities",
                newName: "image_public_id");
        }
    }
}
