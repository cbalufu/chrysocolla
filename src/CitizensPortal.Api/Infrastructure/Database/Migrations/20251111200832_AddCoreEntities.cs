using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitizensPortal.Api.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCoreEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ApplicationNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    FormDataJson = table.Column<string>(type: "TEXT", nullable: false),
                    ReviewedByUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ReviewNotes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AppointmentType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Purpose = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ScheduledTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    RoomNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    AssignedToUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AssignedToUserName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    CancellationReason = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BillNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    BillType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PropertyReference = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    AccountNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bills_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CertificateNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CertificateType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FileUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    VerificationCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    IssuedByUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IssuedByUserName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    RejectionReason = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DocumentType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FileUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsVerified = table.Column<bool>(type: "INTEGER", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    VerifiedByUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    VerificationCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyAlerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AlertType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Severity = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    AffectedAreas = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    InstructionsJson = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IssuedByUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IssuedByUserName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AcknowledgementCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyAlerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    ImageUrls = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    AssignedToUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issues_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActionUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    MetadataJson = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PropertyNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PropertyType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    LandArea = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    BuildingArea = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    NumberOfRooms = table.Column<int>(type: "INTEGER", nullable: true),
                    YearBuilt = table.Column<int>(type: "INTEGER", nullable: true),
                    AssessedValue = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    AnnualTaxAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ZoningClassification = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Citizens_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RequestNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ServiceType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PreferredServiceDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AssignedToUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TicketNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AssignedToUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTickets_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DocumentType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FileUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BillId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PaymentNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TransactionId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ReceiptNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    PaymentGateway = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PaymentGatewayResponse = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertAcknowledgements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AlertId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CitizenId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsSafe = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertAcknowledgements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertAcknowledgements_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertAcknowledgements_EmergencyAlerts_AlertId",
                        column: x => x.AlertId,
                        principalTable: "EmergencyAlerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IssueComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IssueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    IsInternal = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueComments_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TicketId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SenderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SenderName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsStaff = table.Column<bool>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    AttachmentUrls = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketMessages_SupportTickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "SupportTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertAcknowledgements_AlertId",
                table: "AlertAcknowledgements",
                column: "AlertId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertAcknowledgements_CitizenId",
                table: "AlertAcknowledgements",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertAcknowledgements_TenantId_AlertId",
                table: "AlertAcknowledgements",
                columns: new[] { "TenantId", "AlertId" });

            migrationBuilder.CreateIndex(
                name: "IX_AlertAcknowledgements_TenantId_CitizenId_AlertId",
                table: "AlertAcknowledgements",
                columns: new[] { "TenantId", "CitizenId", "AlertId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_ApplicationId",
                table: "ApplicationDocuments",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_TenantId_ApplicationId",
                table: "ApplicationDocuments",
                columns: new[] { "TenantId", "ApplicationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CitizenId",
                table: "Applications",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CreatedAt",
                table: "Applications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TenantId_ApplicationNumber",
                table: "Applications",
                columns: new[] { "TenantId", "ApplicationNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TenantId_CitizenId",
                table: "Applications",
                columns: new[] { "TenantId", "CitizenId" });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TenantId_Status",
                table: "Applications",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CitizenId",
                table: "Appointments",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Status",
                table: "Appointments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TenantId_CitizenId",
                table: "Appointments",
                columns: new[] { "TenantId", "CitizenId" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TenantId_ScheduledDate",
                table: "Appointments",
                columns: new[] { "TenantId", "ScheduledDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_CitizenId",
                table: "Bills",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_DueDate",
                table: "Bills",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_TenantId_BillNumber",
                table: "Bills",
                columns: new[] { "TenantId", "BillNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_TenantId_CitizenId",
                table: "Bills",
                columns: new[] { "TenantId", "CitizenId" });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_TenantId_Status",
                table: "Bills",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_CitizenId",
                table: "Certificates",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_CreatedAt",
                table: "Certificates",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_TenantId_CertificateNumber",
                table: "Certificates",
                columns: new[] { "TenantId", "CertificateNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_TenantId_CitizenId",
                table: "Certificates",
                columns: new[] { "TenantId", "CitizenId" });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_TenantId_VerificationCode",
                table: "Certificates",
                columns: new[] { "TenantId", "VerificationCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CitizenId",
                table: "Documents",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CreatedAt",
                table: "Documents",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TenantId_CitizenId",
                table: "Documents",
                columns: new[] { "TenantId", "CitizenId" });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TenantId_VerificationCode",
                table: "Documents",
                columns: new[] { "TenantId", "VerificationCode" });

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyAlerts_IssuedAt",
                table: "EmergencyAlerts",
                column: "IssuedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyAlerts_Severity",
                table: "EmergencyAlerts",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyAlerts_TenantId_IsActive",
                table: "EmergencyAlerts",
                columns: new[] { "TenantId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_IssueComments_CreatedAt",
                table: "IssueComments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IssueComments_IssueId",
                table: "IssueComments",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueComments_TenantId_IssueId",
                table: "IssueComments",
                columns: new[] { "TenantId", "IssueId" });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_CitizenId",
                table: "Issues",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_CreatedAt",
                table: "Issues",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TenantId_CitizenId",
                table: "Issues",
                columns: new[] { "TenantId", "CitizenId" });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TenantId_Status",
                table: "Issues",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CitizenId",
                table: "Notifications",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedAt",
                table: "Notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TenantId_CitizenId_IsRead",
                table: "Notifications",
                columns: new[] { "TenantId", "CitizenId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BillId",
                table: "Payments",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CitizenId",
                table: "Payments",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatedAt",
                table: "Payments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TenantId_BillId",
                table: "Payments",
                columns: new[] { "TenantId", "BillId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TenantId_CitizenId",
                table: "Payments",
                columns: new[] { "TenantId", "CitizenId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TenantId_PaymentNumber",
                table: "Payments",
                columns: new[] { "TenantId", "PaymentNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CreatedAt",
                table: "Properties",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_OwnerId",
                table: "Properties",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_TenantId_OwnerId",
                table: "Properties",
                columns: new[] { "TenantId", "OwnerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_TenantId_PropertyNumber",
                table: "Properties",
                columns: new[] { "TenantId", "PropertyNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_CitizenId",
                table: "ServiceRequests",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_CreatedAt",
                table: "ServiceRequests",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_TenantId_CitizenId",
                table: "ServiceRequests",
                columns: new[] { "TenantId", "CitizenId" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_TenantId_RequestNumber",
                table: "ServiceRequests",
                columns: new[] { "TenantId", "RequestNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_TenantId_Status",
                table: "ServiceRequests",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_CitizenId",
                table: "SupportTickets",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_CreatedAt",
                table: "SupportTickets",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_TenantId_CitizenId",
                table: "SupportTickets",
                columns: new[] { "TenantId", "CitizenId" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_TenantId_Status",
                table: "SupportTickets",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_TenantId_TicketNumber",
                table: "SupportTickets",
                columns: new[] { "TenantId", "TicketNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_CreatedAt",
                table: "TicketMessages",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_TenantId_TicketId",
                table: "TicketMessages",
                columns: new[] { "TenantId", "TicketId" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_TicketId",
                table: "TicketMessages",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertAcknowledgements");

            migrationBuilder.DropTable(
                name: "ApplicationDocuments");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "IssueComments");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "TicketMessages");

            migrationBuilder.DropTable(
                name: "EmergencyAlerts");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "SupportTickets");
        }
    }
}
