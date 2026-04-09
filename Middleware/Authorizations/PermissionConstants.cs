namespace SchoolManagement.Middleware.Authorizations
{
    public static class PermissionConstants

    {

        //User policy
        public const string AllMighty = "AllMighty";
        public const string ForStudent = "ForStudent";
        public const string CanViewUserDetail = "CanViewUserDetail";
        public const string CanViewCourses = "CanViewCourses";
        public const string TeacherAndAdmin = "TeacherAndAdmin";

        // Award
        public const string ReadAward = "ReadAward";
        public const string CreateAward = "CreateAward";
        public const string UpdateAward = "UpdateAward";
        public const string DeleteAward = "DeleteAward";

        // AwardApproval
        public const string ReadAwardApproval = "ReadAwardApproval";
        public const string CreateAwardApproval = "CreateAwardApproval";
        public const string UpdateAwardApproval = "UpdateAwardApproval";
        public const string DeleteAwardApproval = "DeleteAwardApproval";

        // Course
        public const string ReadCourse = "ReadCourse";
        public const string CreateCourse = "CreateCourse";
        public const string UpdateCourse = "UpdateCourse";
        public const string DeleteCourse = "DeleteCourse";

        // CourseSemester
        public const string ReadCourseSemester = "ReadCourseSemester";
        public const string CreateCourseSemester = "CreateCourseSemester";
        public const string UpdateCourseSemester = "UpdateCourseSemester";
        public const string DeleteCourseSemester = "DeleteCourseSemester";

        // Enrollment
        public const string ReadEnrollment = "ReadEnrollment";
        public const string CreateEnrollment = "CreateEnrollment";
        public const string UpdateEnrollment = "UpdateEnrollment";
        public const string DeleteEnrollment = "DeleteEnrollment";

        // Gpa
        public const string ReadGpa = "ReadGpa";
        public const string CreateGpa = "CreateGpa";
        public const string UpdateGpa = "UpdateGpa";
        public const string DeleteGpa = "DeleteGpa";

        // Grade
        public const string ReadGrade = "ReadGrade";
        public const string CreateGrade = "CreateGrade";
        public const string UpdateGrade = "UpdateGrade";
        public const string DeleteGrade = "DeleteGrade";

        // Role
        public const string ReadRole = "ReadRole";
        public const string CreateRole = "CreateRole";
        public const string UpdateRole = "UpdateRole";
        public const string DeleteRole = "DeleteRole";

        // Semester
        public const string ReadSemester = "ReadSemester";
        public const string CreateSemester = "CreateSemester";
        public const string UpdateSemester = "UpdateSemester";
        public const string DeleteSemester = "DeleteSemester";

        // TeacherCourseSemester
        public const string ReadTeacherCourseSemester = "ReadTeacherCourseSemester";
        public const string CreateTeacherCourseSemester = "CreateTeacherCourseSemester";
        public const string UpdateTeacherCourseSemester = "UpdateTeacherCourseSemester";
        public const string DeleteTeacherCourseSemester = "DeleteTeacherCourseSemester";

        // User
        public const string ReadUser = "ReadUser";
        public const string CreateUser = "CreateUser";
        public const string UpdateUser = "UpdateUser";
        public const string DeleteUser = "DeleteUser";





    }
}
