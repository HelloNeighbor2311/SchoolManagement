namespace SchoolManagement.Middleware.Authorizations.Requirements
{
    using Microsoft.AspNetCore.Authorization;

    public class SameUserOrAdminRequirement: IAuthorizationRequirement
    {
    }
}
