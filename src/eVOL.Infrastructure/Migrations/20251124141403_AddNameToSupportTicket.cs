using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eVOL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToSupportTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_Users_ClaimedById",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_Users_OpenedById",
                table: "SupportTickets");

            migrationBuilder.RenameColumn(
                name: "OpenedById",
                table: "SupportTickets",
                newName: "OpenedBy");

            migrationBuilder.RenameColumn(
                name: "ClaimedById",
                table: "SupportTickets",
                newName: "ClaimedBy");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_OpenedById",
                table: "SupportTickets",
                newName: "IX_SupportTickets_OpenedBy");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_ClaimedById",
                table: "SupportTickets",
                newName: "IX_SupportTickets_ClaimedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_Users_ClaimedBy",
                table: "SupportTickets",
                column: "ClaimedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_Users_OpenedBy",
                table: "SupportTickets",
                column: "OpenedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_Users_ClaimedBy",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_Users_OpenedBy",
                table: "SupportTickets");

            migrationBuilder.RenameColumn(
                name: "OpenedBy",
                table: "SupportTickets",
                newName: "OpenedById");

            migrationBuilder.RenameColumn(
                name: "ClaimedBy",
                table: "SupportTickets",
                newName: "ClaimedById");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_OpenedBy",
                table: "SupportTickets",
                newName: "IX_SupportTickets_OpenedById");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_ClaimedBy",
                table: "SupportTickets",
                newName: "IX_SupportTickets_ClaimedById");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_Users_ClaimedById",
                table: "SupportTickets",
                column: "ClaimedById",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_Users_OpenedById",
                table: "SupportTickets",
                column: "OpenedById",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
