using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class addAwardApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwardApproval_Awards_AwardId",
                table: "AwardApproval");

            migrationBuilder.DropForeignKey(
                name: "FK_AwardApproval_User_TeacherId",
                table: "AwardApproval");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AwardApproval",
                table: "AwardApproval");

            migrationBuilder.DropIndex(
                name: "IX_AwardApproval_TeacherId",
                table: "AwardApproval");

            migrationBuilder.RenameTable(
                name: "AwardApproval",
                newName: "AwardApprovals");

            migrationBuilder.RenameIndex(
                name: "IX_AwardApproval_AwardId_TeacherId",
                table: "AwardApprovals",
                newName: "IX_AwardApprovals_AwardId_TeacherId");
           
            migrationBuilder.DropCheckConstraint(
                 name: "CK_AwardApproval_decision",
                 table: "AwardApprovals");
            
            migrationBuilder.AlterColumn<string>(
                name: "decision",
                table: "AwardApprovals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100);
           
            migrationBuilder.AddCheckConstraint(
               name: "CK_AwardApproval_decision",
               table: "AwardApprovals",
               sql: "decision IN ('Approve','Reject')");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AwardApprovals",
                table: "AwardApprovals",
                column: "ApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_AwardApprovals_TeacherId_AwardId",
                table: "AwardApprovals",
                columns: new[] { "TeacherId", "AwardId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AwardApprovals_Awards_AwardId",
                table: "AwardApprovals",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "AwardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AwardApprovals_User_TeacherId",
                table: "AwardApprovals",
                column: "TeacherId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwardApprovals_Awards_AwardId",
                table: "AwardApprovals");

            migrationBuilder.DropForeignKey(
                name: "FK_AwardApprovals_User_TeacherId",
                table: "AwardApprovals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AwardApprovals",
                table: "AwardApprovals");

            migrationBuilder.DropIndex(
                name: "IX_AwardApprovals_TeacherId_AwardId",
                table: "AwardApprovals");

            migrationBuilder.RenameTable(
                name: "AwardApprovals",
                newName: "AwardApproval");

            migrationBuilder.RenameIndex(
                name: "IX_AwardApprovals_AwardId_TeacherId",
                table: "AwardApproval",
                newName: "IX_AwardApproval_AwardId_TeacherId");
            
            migrationBuilder.DropCheckConstraint(
                 name: "CK_AwardApproval_decision",
                 table: "AwardApprovals");

            migrationBuilder.AlterColumn<int>(
                name: "decision",
                table: "AwardApproval",
                type: "int",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddCheckConstraint(
              name: "CK_AwardApproval_decision",
              table: "AwardApprovals",
              sql: "decision IN (0,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AwardApproval",
                table: "AwardApproval",
                column: "ApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_AwardApproval_TeacherId",
                table: "AwardApproval",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_AwardApproval_Awards_AwardId",
                table: "AwardApproval",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "AwardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AwardApproval_User_TeacherId",
                table: "AwardApproval",
                column: "TeacherId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
