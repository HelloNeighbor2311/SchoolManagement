namespace SchoolManagement.Middleware.Authorizations
{
    public static class RoleConstants
    {
        public const string Admin = "Admin";
        public const string Student = "Student";
        public const string Teacher = "Teacher";

        public const string AdminOrTeacher = "Admin,Teacher";
        public const string AdminOrStudent = "Admin,Student";
        public const string StudentOrTeacher = "Student,Teacher";

        public const string All = "Admin,Teacher,Student";
    }
}
