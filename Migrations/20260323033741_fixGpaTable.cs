using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class fixGpaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gpas_Semesters_SemesterId",
                table: "Gpas");

            migrationBuilder.DropIndex(
                name: "IX_Gpas_SemesterId",
                table: "Gpas");

            migrationBuilder.DropCheckConstraint(
        name: "CK_Gpa_rank",
        table: "Gpas");

           
            migrationBuilder.AlterColumn<string>(
                name: "rank",
                table: "Gpas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Gpa_rank",
                table: "Gpas",
                sql: "rank IN ('Excellent','Good','Average','Bad')");

            migrationBuilder.CreateIndex(
                name: "IX_Gpas_SemesterId",
                table: "Gpas",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gpas_Semesters_SemesterId",
                table: "Gpas",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gpas_Semesters_SemesterId",
                table: "Gpas");

            migrationBuilder.DropIndex(
                name: "IX_Gpas_SemesterId",
                table: "Gpas");

            migrationBuilder.AlterColumn<int>(
                name: "rank",
                table: "Gpas",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gpas_SemesterId",
                table: "Gpas",
                column: "SemesterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpas_Semesters_SemesterId",
                table: "Gpas",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
