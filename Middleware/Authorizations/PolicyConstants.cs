namespace SchoolManagement.Middleware.Authorizations
{
    public static class PolicyConstants
    {

        //User policy
        public const string AllMighty = "AllMighty";
        public const string ForStudent = "ForStudent";
        public const string CanViewUserDetail = "CanViewUserDetail";
        public const string CanViewCourses = "CanViewCourses";
        public const string TeacherAndAdmin = "TeacherAndAdmin";
    }
}
