using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "domain_clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_domain_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "loading_documents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    document_number = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    date_only = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loading_documents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "measure_units",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_measure_units", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resources",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dispatch_documents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    document_number = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    date_only = table.Column<DateOnly>(type: "date", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dispatch_documents", x => x.id);
                    table.ForeignKey(
                        name: "fk_dispatch_documents_domain_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "domain_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "balances",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    measure_unit_id = table.Column<int>(type: "integer", nullable: false),
                    resource_id = table.Column<int>(type: "integer", nullable: false),
                    domain_resource_id = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_balances", x => x.id);
                    table.ForeignKey(
                        name: "fk_balances_measure_units_measure_unit_id",
                        column: x => x.measure_unit_id,
                        principalTable: "measure_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_balances_resources_domain_resource_id",
                        column: x => x.domain_resource_id,
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dispatch_document_resources",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    resource_id = table.Column<int>(type: "integer", nullable: false),
                    domain_resource_id = table.Column<int>(type: "integer", nullable: false),
                    measure_unit_id = table.Column<int>(type: "integer", nullable: false),
                    dispatch_document_id = table.Column<int>(type: "integer", nullable: false),
                    balance_id = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dispatch_document_resources", x => x.id);
                    table.ForeignKey(
                        name: "fk_dispatch_document_resources_balances_balance_id",
                        column: x => x.balance_id,
                        principalTable: "balances",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_dispatch_document_resources_dispatch_documents_dispatch_doc",
                        column: x => x.dispatch_document_id,
                        principalTable: "dispatch_documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_dispatch_document_resources_measure_units_measure_unit_id",
                        column: x => x.measure_unit_id,
                        principalTable: "measure_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_dispatch_document_resources_resources_domain_resource_id",
                        column: x => x.domain_resource_id,
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "loading_document_resources",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    count = table.Column<int>(type: "integer", nullable: false),
                    resource_id = table.Column<int>(type: "integer", nullable: false),
                    domain_resource_id = table.Column<int>(type: "integer", nullable: false),
                    loading_document_id = table.Column<int>(type: "integer", nullable: false),
                    measure_unit_id = table.Column<int>(type: "integer", nullable: false),
                    balance_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loading_document_resources", x => x.id);
                    table.ForeignKey(
                        name: "fk_loading_document_resources_balances_balance_id",
                        column: x => x.balance_id,
                        principalTable: "balances",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_loading_document_resources_loading_documents_loading_docume",
                        column: x => x.loading_document_id,
                        principalTable: "loading_documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_loading_document_resources_measure_units_measure_unit_id",
                        column: x => x.measure_unit_id,
                        principalTable: "measure_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_loading_document_resources_resources_domain_resource_id",
                        column: x => x.domain_resource_id,
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_balances_domain_resource_id",
                table: "balances",
                column: "domain_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_balances_measure_unit_id",
                table: "balances",
                column: "measure_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_dispatch_document_resources_balance_id",
                table: "dispatch_document_resources",
                column: "balance_id");

            migrationBuilder.CreateIndex(
                name: "ix_dispatch_document_resources_dispatch_document_id",
                table: "dispatch_document_resources",
                column: "dispatch_document_id");

            migrationBuilder.CreateIndex(
                name: "ix_dispatch_document_resources_domain_resource_id",
                table: "dispatch_document_resources",
                column: "domain_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_dispatch_document_resources_measure_unit_id",
                table: "dispatch_document_resources",
                column: "measure_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_dispatch_documents_client_id",
                table: "dispatch_documents",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_dispatch_documents_document_number",
                table: "dispatch_documents",
                column: "document_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_domain_clients_name",
                table: "domain_clients",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loading_document_resources_balance_id",
                table: "loading_document_resources",
                column: "balance_id");

            migrationBuilder.CreateIndex(
                name: "ix_loading_document_resources_domain_resource_id",
                table: "loading_document_resources",
                column: "domain_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_loading_document_resources_loading_document_id",
                table: "loading_document_resources",
                column: "loading_document_id");

            migrationBuilder.CreateIndex(
                name: "ix_loading_document_resources_measure_unit_id",
                table: "loading_document_resources",
                column: "measure_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_loading_documents_document_number",
                table: "loading_documents",
                column: "document_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_measure_units_name",
                table: "measure_units",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_resources_name",
                table: "resources",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dispatch_document_resources");

            migrationBuilder.DropTable(
                name: "loading_document_resources");

            migrationBuilder.DropTable(
                name: "dispatch_documents");

            migrationBuilder.DropTable(
                name: "balances");

            migrationBuilder.DropTable(
                name: "loading_documents");

            migrationBuilder.DropTable(
                name: "domain_clients");

            migrationBuilder.DropTable(
                name: "measure_units");

            migrationBuilder.DropTable(
                name: "resources");
        }
    }
}
