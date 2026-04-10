namespace SchoolManagement.Middleware.Authorizations
{
    public static class PolicyConstants

    {

        //User policy
        public const string ForAdminOnly = "ForAdminOnly";
        public const string ForStudent = "ForStudentOnly";
        public const string AuthenticatedUsers = "AuthenticatedUsers";
        public const string TeacherAndAdmin = "TeacherAndAdmin";
        public const string TeacherAndStudent = "TeacherAndStudent";
        public const string StudentAndAdmin = "StudentAndAdmin";



        //Custom policies
        public const string OnlyUserCanViewUserDetail = "OnlyUserCanViewUserDetail";
    }
}
