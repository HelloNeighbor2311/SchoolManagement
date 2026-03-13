using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSemester_Course_CourseId",
                table: "CourseSemester");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherCourseSemester_CourseSemester_CourseSemsterId",
                table: "TeacherCourseSemester");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.RenameColumn(
                name: "CourseSemsterId",
                table: "TeacherCourseSemester",
                newName: "CourseSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherCourseSemester_CourseSemsterId",
                table: "TeacherCourseSemester",
                newName: "IX_TeacherCourseSemester_CourseSemesterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseName",
                table: "Courses",
                column: "CourseName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSemester_Courses_CourseId",
                table: "CourseSemester",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherCourseSemester_CourseSemester_CourseSemesterId",
                table: "TeacherCourseSemester",
                column: "CourseSemesterId",
                principalTable: "CourseSemester",
                principalColumn: "CourseSemesterId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSemester_Courses_CourseId",
                table: "CourseSemester");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherCourseSemester_CourseSemester_CourseSemesterId",
                table: "TeacherCourseSemester");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseName",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.RenameColumn(
                name: "CourseSemesterId",
                table: "TeacherCourseSemester",
                newName: "CourseSemsterId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherCourseSemester_CourseSemesterId",
                table: "TeacherCourseSemester",
                newName: "IX_TeacherCourseSemester_CourseSemsterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSemester_Course_CourseId",
                table: "CourseSemester",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherCourseSemester_CourseSemester_CourseSemsterId",
                table: "TeacherCourseSemester",
                column: "CourseSemsterId",
                principalTable: "CourseSemester",
                principalColumn: "CourseSemesterId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
