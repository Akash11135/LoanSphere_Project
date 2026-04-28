using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagement.Migrations
{
    /// <inheritdoc />
    public partial class DualReviewWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminReviewReason",
                table: "Loans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AdminReviewedAt",
                table: "Loans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminReviewStatus",
                table: "Loans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<string>(
                name: "ManagerReviewReason",
                table: "Loans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManagerReviewedAt",
                table: "Loans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerReviewStatus",
                table: "Loans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminReviewReason",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "AdminReviewedAt",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "AdminReviewStatus",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "ManagerReviewReason",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "ManagerReviewedAt",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "ManagerReviewStatus",
                table: "Loans");
        }
    }
}
