using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class addAwardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Award_Gpas_GpaId",
                table: "Award");

            migrationBuilder.DropForeignKey(
                name: "FK_Award_User_StudentId",
                table: "Award");

            migrationBuilder.DropForeignKey(
                name: "FK_AwardApproval_Award_AwardId",
                table: "AwardApproval");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Award",
                table: "Award");

            migrationBuilder.DropIndex(
                name: "IX_Award_GpaId",
                table: "Award");

            migrationBuilder.DropIndex(
                name: "IX_Award_StudentId",
                table: "Award");
            migrationBuilder.DropCheckConstraint(
                name: "CK_Award_Status",
                table: "Award");
            migrationBuilder.RenameTable(
                name: "Award",
                newName: "Awards");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Awards",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RequireApproval",
                table: "Awards",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredDate",
                table: "Awards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Awards",
                table: "Awards",
                column: "AwardId");
            migrationBuilder.AddCheckConstraint(
                name: "CK_Award_Status",
                table: "Awards",
                sql: "Status IN ('Approved','Rejected','Pending')");

            migrationBuilder.CreateIndex(
                name: "IX_Awards_GpaId",
                table: "Awards",
                column: "GpaId");

            migrationBuilder.CreateIndex(
                name: "IX_Awards_StudentId",
                table: "Awards",
                column: "StudentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AwardApproval_Awards_AwardId",
                table: "AwardApproval",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "AwardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Awards_Gpas_GpaId",
                table: "Awards",
                column: "GpaId",
                principalTable: "Gpas",
                principalColumn: "GPAId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Awards_User_StudentId",
                table: "Awards",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwardApproval_Awards_AwardId",
                table: "AwardApproval");

            migrationBuilder.DropForeignKey(
                name: "FK_Awards_Gpas_GpaId",
                table: "Awards");

            migrationBuilder.DropForeignKey(
                name: "FK_Awards_User_StudentId",
                table: "Awards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Awards",
                table: "Awards");

            migrationBuilder.DropIndex(
                name: "IX_Awards_GpaId",
                table: "Awards");

            migrationBuilder.DropIndex(
                name: "IX_Awards_StudentId",
                table: "Awards");

            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "Awards");

            migrationBuilder.RenameTable(
                name: "Awards",
                newName: "Award");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "Award",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "RequireApproval",
                table: "Award",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Award",
                table: "Award",
                column: "AwardId");

            migrationBuilder.CreateIndex(
                name: "IX_Award_GpaId",
                table: "Award",
                column: "GpaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Award_StudentId",
                table: "Award",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Award_Gpas_GpaId",
                table: "Award",
                column: "GpaId",
                principalTable: "Gpas",
                principalColumn: "GPAId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Award_User_StudentId",
                table: "Award",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AwardApproval_Award_AwardId",
                table: "AwardApproval",
                column: "AwardId",
                principalTable: "Award",
                principalColumn: "AwardId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
