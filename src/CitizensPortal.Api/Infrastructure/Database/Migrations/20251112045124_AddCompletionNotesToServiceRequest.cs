using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitizensPortal.Api.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletionNotesToServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompletionNotes",
                table: "ServiceRequests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionNotes",
                table: "ServiceRequests");
        }
    }
}
