using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixedTableGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Enrollments_EnrollmentId",
                table: "Grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grade",
                table: "Grade");

            migrationBuilder.RenameTable(
                name: "Grade",
                newName: "Grades");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_EnrollmentId",
                table: "Grades",
                newName: "IX_Grades_EnrollmentId");

            migrationBuilder.AlterColumn<double>(
                name: "SecondGrade",
                table: "Grades",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FirstGrade",
                table: "Grades",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FinalGrade",
                table: "Grades",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grades",
                table: "Grades",
                column: "GradeId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Grade_FinalGrade",
                table: "Grades",
                sql: "FinalGrade IS NULL OR (FinalGrade >= 0 AND FinalGrade <= 10)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Grade_SecondGrade",
                table: "Grades",
                sql: "SecondGrade IS NULL OR (SecondGrade >= 0 AND SecondGrade <= 10)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Grades_FirstGrade",
                table: "Grades",
                sql: "FirstGrade IS NULL OR (FirstGrade >= 0 AND FirstGrade <= 10)");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Enrollments_EnrollmentId",
                table: "Grades",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "EnrollmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Enrollments_EnrollmentId",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grades",
                table: "Grades");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Grade_FinalGrade",
                table: "Grades");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Grade_SecondGrade",
                table: "Grades");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Grades_FirstGrade",
                table: "Grades");

            migrationBuilder.RenameTable(
                name: "Grades",
                newName: "Grade");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_EnrollmentId",
                table: "Grade",
                newName: "IX_Grade_EnrollmentId");

            migrationBuilder.AlterColumn<int>(
                name: "SecondGrade",
                table: "Grade",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FirstGrade",
                table: "Grade",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FinalGrade",
                table: "Grade",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grade",
                table: "Grade",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Enrollments_EnrollmentId",
                table: "Grade",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "EnrollmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
