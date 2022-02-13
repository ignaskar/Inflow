using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inflow.Modules.Customers.Core.DAL.Migrations
{
    public partial class Customer_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "customers");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Nationality = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Identity = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Address",
                schema: "customers",
                table: "Customers",
                column: "Address");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                schema: "customers",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_FullName",
                schema: "customers",
                table: "Customers",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Identity",
                schema: "customers",
                table: "Customers",
                column: "Identity");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name",
                schema: "customers",
                table: "Customers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Nationality",
                schema: "customers",
                table: "Customers",
                column: "Nationality");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Notes",
                schema: "customers",
                table: "Customers",
                column: "Notes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers",
                schema: "customers");
        }
    }
}
