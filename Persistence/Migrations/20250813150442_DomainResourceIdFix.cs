using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DomainResourceIdFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resource_id",
                table: "loading_document_resources");

            migrationBuilder.DropColumn(
                name: "resource_id",
                table: "dispatch_document_resources");

            migrationBuilder.DropColumn(
                name: "resource_id",
                table: "balances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "resource_id",
                table: "loading_document_resources",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "resource_id",
                table: "dispatch_document_resources",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "resource_id",
                table: "balances",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
