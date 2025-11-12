using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitizensPortal.Api.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthenticationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "Citizens",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Citizens",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Citizens",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Citizens",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "Citizens");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Citizens");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Citizens");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Citizens");
        }
    }
}
