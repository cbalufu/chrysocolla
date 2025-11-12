using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitizensPortal.Api.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCitizensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Citizens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NationalId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_TenantId_Email",
                table: "Citizens",
                columns: new[] { "TenantId", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_TenantId_NationalId",
                table: "Citizens",
                columns: new[] { "TenantId", "NationalId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citizens");
        }
    }
}
