using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitizensPortal.Api.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_CitizenId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_TenantId_CitizenId_IsRead",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ActionUrl",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "MetadataJson",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ReadAt",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Notifications",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "Notifications",
                newName: "ReadDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Notifications",
                newName: "SentDate");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_CreatedAt",
                table: "Notifications",
                newName: "IX_Notifications_SentDate");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Notifications",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CitizenId_IsRead",
                table: "Notifications",
                columns: new[] { "CitizenId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TenantId_CitizenId",
                table: "Notifications",
                columns: new[] { "TenantId", "CitizenId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_CitizenId_IsRead",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_TenantId_CitizenId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Notifications",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "SentDate",
                table: "Notifications",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ReadDate",
                table: "Notifications",
                newName: "SentAt");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_SentDate",
                table: "Notifications",
                newName: "IX_Notifications_CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "ActionUrl",
                table: "Notifications",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Notifications",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetadataJson",
                table: "Notifications",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadAt",
                table: "Notifications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CitizenId",
                table: "Notifications",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TenantId_CitizenId_IsRead",
                table: "Notifications",
                columns: new[] { "TenantId", "CitizenId", "IsRead" });
        }
    }
}
