using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class ConfigUniqueKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeacherCourseSemester_TeacherId",
                table: "TeacherCourseSemester");

            migrationBuilder.DropIndex(
                name: "IX_Enrollment_StudentId",
                table: "Enrollment");

            migrationBuilder.DropIndex(
                name: "IX_CourseSemester_CourseId",
                table: "CourseSemester");

            migrationBuilder.DropIndex(
                name: "IX_AwardApproval_AwardId",
                table: "AwardApproval");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Award",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourseSemester_TeacherId_CourseSemesterId",
                table: "TeacherCourseSemester",
                columns: new[] { "TeacherId", "CourseSemesterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_StudentId_CourseSemesterId",
                table: "Enrollment",
                columns: new[] { "StudentId", "CourseSemesterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseSemester_CourseId_SemesterId",
                table: "CourseSemester",
                columns: new[] { "CourseId", "SemesterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AwardApproval_AwardId_TeacherId",
                table: "AwardApproval",
                columns: new[] { "AwardId", "TeacherId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeacherCourseSemester_TeacherId_CourseSemesterId",
                table: "TeacherCourseSemester");

            migrationBuilder.DropIndex(
                name: "IX_Enrollment_StudentId_CourseSemesterId",
                table: "Enrollment");

            migrationBuilder.DropIndex(
                name: "IX_CourseSemester_CourseId_SemesterId",
                table: "CourseSemester");

            migrationBuilder.DropIndex(
                name: "IX_AwardApproval_AwardId_TeacherId",
                table: "AwardApproval");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Award");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourseSemester_TeacherId",
                table: "TeacherCourseSemester",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_StudentId",
                table: "Enrollment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSemester_CourseId",
                table: "CourseSemester",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AwardApproval_AwardId",
                table: "AwardApproval",
                column: "AwardId");
        }
    }
}
