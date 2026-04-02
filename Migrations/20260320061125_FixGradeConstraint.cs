using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixGradeConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSemester_Courses_CourseId",
                table: "CourseSemester");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSemester_Semester_SemesterId",
                table: "CourseSemester");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_CourseSemester_CourseSemesterId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_User_StudentId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpa_Semester_SemesterId",
                table: "Gpa");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Enrollment_EnrollmentId",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherCourseSemester_CourseSemester_CourseSemesterId",
                table: "TeacherCourseSemester");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherCourseSemester_User_TeacherId",
                table: "TeacherCourseSemester");

            migrationBuilder.DropIndex(
                name: "IX_Grade_EnrollmentId",
                table: "Grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherCourseSemester",
                table: "TeacherCourseSemester");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Semester",
                table: "Semester");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollment",
                table: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSemester",
                table: "CourseSemester");

            migrationBuilder.RenameTable(
                name: "TeacherCourseSemester",
                newName: "TeacherCourseSemesters");

            migrationBuilder.RenameTable(
                name: "Semester",
                newName: "Semesters");

            migrationBuilder.RenameTable(
                name: "Enrollment",
                newName: "Enrollments");

            migrationBuilder.RenameTable(
                name: "CourseSemester",
                newName: "CourseSemesters");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherCourseSemester_TeacherId_CourseSemesterId",
                table: "TeacherCourseSemesters",
                newName: "IX_TeacherCourseSemesters_TeacherId_CourseSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherCourseSemester_CourseSemesterId",
                table: "TeacherCourseSemesters",
                newName: "IX_TeacherCourseSemesters_CourseSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_StudentId_CourseSemesterId",
                table: "Enrollments",
                newName: "IX_Enrollments_StudentId_CourseSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_CourseSemesterId",
                table: "Enrollments",
                newName: "IX_Enrollments_CourseSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSemester_SemesterId",
                table: "CourseSemesters",
                newName: "IX_CourseSemesters_SemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSemester_CourseId_SemesterId",
                table: "CourseSemesters",
                newName: "IX_CourseSemesters_CourseId_SemesterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherCourseSemesters",
                table: "TeacherCourseSemesters",
                column: "TeacherCourseSemesterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Semesters",
                table: "Semesters",
                column: "SemesterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments",
                column: "EnrollmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSemesters",
                table: "CourseSemesters",
                column: "CourseSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_EnrollmentId",
                table: "Grade",
                column: "EnrollmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSemesters_Courses_CourseId",
                table: "CourseSemesters",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSemesters_Semesters_SemesterId",
                table: "CourseSemesters",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_CourseSemesters_CourseSemesterId",
                table: "Enrollments",
                column: "CourseSemesterId",
                principalTable: "CourseSemesters",
                principalColumn: "CourseSemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_User_StudentId",
                table: "Enrollments",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpa_Semesters_SemesterId",
                table: "Gpa",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Enrollments_EnrollmentId",
                table: "Grade",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "EnrollmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherCourseSemesters_CourseSemesters_CourseSemesterId",
                table: "TeacherCourseSemesters",
                column: "CourseSemesterId",
                principalTable: "CourseSemesters",
                principalColumn: "CourseSemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherCourseSemesters_User_TeacherId",
                table: "TeacherCourseSemesters",
                column: "TeacherId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSemesters_Courses_CourseId",
                table: "CourseSemesters");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSemesters_Semesters_SemesterId",
                table: "CourseSemesters");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_CourseSemesters_CourseSemesterId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_User_StudentId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpa_Semesters_SemesterId",
                table: "Gpa");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Enrollments_EnrollmentId",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherCourseSemesters_CourseSemesters_CourseSemesterId",
                table: "TeacherCourseSemesters");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherCourseSemesters_User_TeacherId",
                table: "TeacherCourseSemesters");

            migrationBuilder.DropIndex(
                name: "IX_Grade_EnrollmentId",
                table: "Grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherCourseSemesters",
                table: "TeacherCourseSemesters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Semesters",
                table: "Semesters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSemesters",
                table: "CourseSemesters");

            migrationBuilder.RenameTable(
                name: "TeacherCourseSemesters",
                newName: "TeacherCourseSemester");

            migrationBuilder.RenameTable(
                name: "Semesters",
                newName: "Semester");

            migrationBuilder.RenameTable(
                name: "Enrollments",
                newName: "Enrollment");

            migrationBuilder.RenameTable(
                name: "CourseSemesters",
                newName: "CourseSemester");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherCourseSemesters_TeacherId_CourseSemesterId",
                table: "TeacherCourseSemester",
                newName: "IX_TeacherCourseSemester_TeacherId_CourseSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherCourseSemesters_CourseSemesterId",
                table: "TeacherCourseSemester",
                newName: "IX_TeacherCourseSemester_CourseSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollments_StudentId_CourseSemesterId",
                table: "Enrollment",
                newName: "IX_Enrollment_StudentId_CourseSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollments_CourseSemesterId",
                table: "Enrollment",
                newName: "IX_Enrollment_CourseSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSemesters_SemesterId",
                table: "CourseSemester",
                newName: "IX_CourseSemester_SemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSemesters_CourseId_SemesterId",
                table: "CourseSemester",
                newName: "IX_CourseSemester_CourseId_SemesterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherCourseSemester",
                table: "TeacherCourseSemester",
                column: "TeacherCourseSemesterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Semester",
                table: "Semester",
                column: "SemesterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollment",
                table: "Enrollment",
                column: "EnrollmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSemester",
                table: "CourseSemester",
                column: "CourseSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_EnrollmentId",
                table: "Grade",
                column: "EnrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSemester_Courses_CourseId",
                table: "CourseSemester",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSemester_Semester_SemesterId",
                table: "CourseSemester",
                column: "SemesterId",
                principalTable: "Semester",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_CourseSemester_CourseSemesterId",
                table: "Enrollment",
                column: "CourseSemesterId",
                principalTable: "CourseSemester",
                principalColumn: "CourseSemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_User_StudentId",
                table: "Enrollment",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpa_Semester_SemesterId",
                table: "Gpa",
                column: "SemesterId",
                principalTable: "Semester",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Enrollment_EnrollmentId",
                table: "Grade",
                column: "EnrollmentId",
                principalTable: "Enrollment",
                principalColumn: "EnrollmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherCourseSemester_CourseSemester_CourseSemesterId",
                table: "TeacherCourseSemester",
                column: "CourseSemesterId",
                principalTable: "CourseSemester",
                principalColumn: "CourseSemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherCourseSemester_User_TeacherId",
                table: "TeacherCourseSemester",
                column: "TeacherId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
