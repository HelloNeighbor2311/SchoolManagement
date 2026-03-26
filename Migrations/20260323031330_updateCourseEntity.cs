using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class updateCourseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Award_Gpa_GpaId",
                table: "Award");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpa_Semesters_SemesterId",
                table: "Gpa");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpa_User_StudentId",
                table: "Gpa");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gpa",
                table: "Gpa");

            migrationBuilder.DropIndex(
                name: "IX_Gpa_StudentId",
                table: "Gpa");

            migrationBuilder.RenameTable(
                name: "Gpa",
                newName: "Gpas");

            migrationBuilder.RenameIndex(
                name: "IX_Gpa_SemesterId",
                table: "Gpas",
                newName: "IX_Gpas_SemesterId");

            migrationBuilder.AddColumn<int>(
                name: "Credits",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "rank",
                table: "Gpas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "gpa",
                table: "Gpas",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gpas",
                table: "Gpas",
                column: "GPAId");

            migrationBuilder.CreateIndex(
                name: "IX_Gpas_StudentId_SemesterId",
                table: "Gpas",
                columns: new[] { "StudentId", "SemesterId" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Gpa_gpa",
                table: "Gpas",
                sql: "gpa IS NULL OR (gpa >= 0 AND gpa <= 4)");

            migrationBuilder.AddForeignKey(
                name: "FK_Award_Gpas_GpaId",
                table: "Award",
                column: "GpaId",
                principalTable: "Gpas",
                principalColumn: "GPAId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpas_Semesters_SemesterId",
                table: "Gpas",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpas_User_StudentId",
                table: "Gpas",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Award_Gpas_GpaId",
                table: "Award");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpas_Semesters_SemesterId",
                table: "Gpas");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpas_User_StudentId",
                table: "Gpas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gpas",
                table: "Gpas");

            migrationBuilder.DropIndex(
                name: "IX_Gpas_StudentId_SemesterId",
                table: "Gpas");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Gpa_gpa",
                table: "Gpas");

            migrationBuilder.DropColumn(
                name: "Credits",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Gpas",
                newName: "Gpa");

            migrationBuilder.RenameIndex(
                name: "IX_Gpas_SemesterId",
                table: "Gpa",
                newName: "IX_Gpa_SemesterId");

            migrationBuilder.AlterColumn<int>(
                name: "rank",
                table: "Gpa",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "gpa",
                table: "Gpa",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gpa",
                table: "Gpa",
                column: "GPAId");

            migrationBuilder.CreateIndex(
                name: "IX_Gpa_StudentId",
                table: "Gpa",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Award_Gpa_GpaId",
                table: "Award",
                column: "GpaId",
                principalTable: "Gpa",
                principalColumn: "GPAId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpa_Semesters_SemesterId",
                table: "Gpa",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpa_User_StudentId",
                table: "Gpa",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
