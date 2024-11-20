using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GorevYonetimSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Duties_Users_UserId",
                table: "Duties");

            migrationBuilder.DropIndex(
                name: "IX_Duties_UserId",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Duties");

            migrationBuilder.CreateTable(
                name: "UserDuties",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DutyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserDutyId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDuties", x => new { x.UserId, x.DutyId });
                    table.ForeignKey(
                        name: "FK_UserDuties_Duties_DutyId",
                        column: x => x.DutyId,
                        principalTable: "Duties",
                        principalColumn: "DutyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDuties_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDuties_DutyId",
                table: "UserDuties",
                column: "DutyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDuties");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Duties",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Duties_UserId",
                table: "Duties",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Duties_Users_UserId",
                table: "Duties",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
