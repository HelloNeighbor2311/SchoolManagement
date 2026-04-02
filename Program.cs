using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SchoolManagement.Datas;
using SchoolManagement.Middleware;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Middleware.Authorizations.Handlers;
using SchoolManagement.Middleware.Authorizations.Requirements;
using SchoolManagement.Repositories;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services;
using SchoolManagement.Services.Interfaces;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)

    .Enrich.FromLogContext()// Enable BeginScope
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .Enrich.WithThreadId()
    .Enrich.WithProperty("Application", "SchoolManagement")
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}]{NewLine}" +
                       "{Message:lj}{NewLine}" +
                       "{Properties:j}{NewLine}" +
                       "{Exception}"
    )
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30, // Keep 30 days
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}"
    )
    .CreateLogger();
builder.Host.UseSerilog();
try
{
    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
    {
        //Connection path
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.LogTo(
        Console.WriteLine,                    // Hoặc custom logger
        new[] { DbLoggerCategory.Database.Command.Name },  // Chỉ log SQL commands
        LogLevel.Information                  // Log level
    );

        // ✅ HIỂN THỊ SENSITIVE DATA (chỉ development)
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();  // Show parameter values
            options.EnableDetailedErrors();        // Show detailed errors
        }
    });
    //AutoMapper
    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddMaps(typeof(Program).Assembly);
    });
    //Middleware
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

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
        opt.AddPolicy(PolicyConstants.ForStudent, policy => policy.RequireRole(RoleConstants.Student));
        opt.AddPolicy(PolicyConstants.CanViewCourses, policy => policy.RequireRole(RoleConstants.Admin, RoleConstants.Student, RoleConstants.Teacher));
        opt.AddPolicy(PolicyConstants.TeacherAndAdmin, policy => policy.RequireRole(RoleConstants.Admin, RoleConstants.Teacher));
        //Custom requirement policies
        opt.AddPolicy(PolicyConstants.CanViewUserDetail, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.AddRequirements(new SameUserOrAdminRequirement());
        });
    });

    //Convert string to enum
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters
                .Add(new JsonStringEnumConverter()); // ← toàn bộ enum trong project
        });

    builder.Services.AddScoped<IAuthorizationHandler, SameUserOrAdminHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, StudentDataOwnerHandler>();

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
    builder.Services.AddScoped<ICourseRepository, CourseRepository>();
    builder.Services.AddScoped<ITeacherCourseSemesterRepository, TeacherCourseSemesterRepository>();
    builder.Services.AddScoped<ICourseSemesterRepository, CourseSemesterRepository>();
    builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
    builder.Services.AddScoped<IGradeRepository, GradeRepository>();
    builder.Services.AddScoped<IGpaRepository, GpaRepository>();
    builder.Services.AddScoped<IAwardRepository, AwardRepository>();
    builder.Services.AddScoped<IAwardApprovalRepository, AwardApprovalRepository>();

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IJWTService, JWTService>();
    builder.Services.AddScoped<ICourseService, CourseService>();
    builder.Services.AddScoped<ISemesterService, SemesterService>();
    builder.Services.AddScoped<ICourseSemesterService, CourseSemesterService>();
    builder.Services.AddScoped<ITeacherCourseSemesterService, TeacherCourseSemesterService>();
    builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
    builder.Services.AddScoped<IGradeService, GradeService>();
    builder.Services.AddScoped<IGpaService, GpaService>();
    builder.Services.AddScoped<IAwardService, AwardService>();
    builder.Services.AddScoped<IAwardApprovalService, AwardApprovalService>();
    var app = builder.Build();
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
            diagnosticContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress?.ToString());
        };
    });
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();
    app.UseExceptionHandler();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}catch(Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
