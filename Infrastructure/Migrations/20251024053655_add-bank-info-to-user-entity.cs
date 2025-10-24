using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addbankinfotouserentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bank_account_number",
                table: "users",
                type: "nvarchar(64)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bank_holder_name",
                table: "users",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bank_name",
                table: "users",
                type: "nvarchar(100)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bank_account_number",
                table: "users");

            migrationBuilder.DropColumn(
                name: "bank_holder_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "bank_name",
                table: "users");
        }
    }
}
