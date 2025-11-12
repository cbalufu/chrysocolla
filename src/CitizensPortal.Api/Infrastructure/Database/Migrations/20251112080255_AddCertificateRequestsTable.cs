using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitizensPortal.Api.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificateRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CertificateRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CertificateType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Purpose = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    DeliveryMethod = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ApplicationFee = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CollectionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RejectionReason = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateRequests_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRequests_CitizenId",
                table: "CertificateRequests",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRequests_CreatedAt",
                table: "CertificateRequests",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRequests_ReferenceNumber",
                table: "CertificateRequests",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRequests_Status_IsPaid",
                table: "CertificateRequests",
                columns: new[] { "Status", "IsPaid" });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRequests_TenantId_CitizenId",
                table: "CertificateRequests",
                columns: new[] { "TenantId", "CitizenId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateRequests");
        }
    }
}
