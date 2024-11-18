using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freelance.Migrations
{
    /// <inheritdoc />
    public partial class Proposalaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SelectedProposalId",
                table: "ProjectPosts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedProposalId1",
                table: "ProjectPosts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPosts_SelectedProposalId1",
                table: "ProjectPosts",
                column: "SelectedProposalId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPosts_Proposals_SelectedProposalId1",
                table: "ProjectPosts",
                column: "SelectedProposalId1",
                principalTable: "Proposals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPosts_Proposals_SelectedProposalId1",
                table: "ProjectPosts");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPosts_SelectedProposalId1",
                table: "ProjectPosts");

            migrationBuilder.DropColumn(
                name: "SelectedProposalId",
                table: "ProjectPosts");

            migrationBuilder.DropColumn(
                name: "SelectedProposalId1",
                table: "ProjectPosts");
        }
    }
}
