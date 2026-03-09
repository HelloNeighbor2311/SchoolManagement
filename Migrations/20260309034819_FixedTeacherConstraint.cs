using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixedTeacherConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Teacher_NoEnrollYear",
                table: "User");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Teacher_NoEnrollYear",
                table: "User",
                sql: "UserType != 'Teacher' OR EnrollYear IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Teacher_NoEnrollYear",
                table: "User");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Teacher_NoEnrollYear",
                table: "User",
                sql: "UserType != 'Teacher' OR Speciality IS NULL");
        }
    }
}
