using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SchoolManagement.Datas;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Middleware.Authorizations.Handlers;
using SchoolManagement.Middleware.Authorizations.Requirements;
using SchoolManagement.Repositories;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services;
using SchoolManagement.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(p => p.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
});
//JWT Authentication
var jwtSetting = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSetting["SecretKey"]!;
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSetting["Issuer"],
        ValidAudience = jwtSetting["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
    opt.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("""{"statusCode":401,"Message":"Unauthorized. Please provide a valid token."}""");
        },
        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("""{"statusCode":403,"Message":"Forbidden. You don't have permission."}""");
        }
    };
});

builder.Services.AddAuthorization(opt =>
{
    //Role-based policies
    opt.AddPolicy(PolicyConstants.AllMighty, policy => policy.RequireRole(RoleConstants.Admin));
    opt.AddPolicy(PolicyConstants.CanViewCourses, policy => policy.RequireRole(RoleConstants.Admin, RoleConstants.Student, RoleConstants.Teacher));
    //Custom requirement policies
    opt.AddPolicy(PolicyConstants.CanViewUserDetail, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new SameUserOrAdminRequirement());
    });
});

builder.Services.AddScoped<IAuthorizationHandler, SameUserOrAdminHandler>();
builder.Services.AddScoped<IAuthorizationHandler, StudentDataOwnerHandler>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ITeacherCourseSemesterRepository, TeacherCourseSemesterRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<ITeacherCourseSemesterService, TeacherCourseSemesterService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
