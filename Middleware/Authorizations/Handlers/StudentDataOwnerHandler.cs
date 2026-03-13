using Microsoft.AspNetCore.Authorization;
using SchoolManagement.Middleware.Authorizations.Requirements;

namespace SchoolManagement.Middleware.Authorizations.Handlers
{
    public class StudentDataOwnerHandler : BaseAuthorizationHandler<StudentDataOwnerRequirement>
    {
        public StudentDataOwnerHandler(IHttpContextAccessor context): base(context){}
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StudentDataOwnerRequirement requirement)
        {
           if(IsAdmin(context) || IsTeacher(context))
           {
               context.Succeed(requirement);
               return Task.CompletedTask;
           }
            if (!IsStudent(context)) return Task.CompletedTask;
            if (!TryGetCurrentUserId(context, out int currentStudentId)) return Task.CompletedTask;
            if(TryGetRouteId("studentId",out int targetStudentId) && currentStudentId == targetStudentId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
