using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
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
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json.Serialization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);


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
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    // Configure OpenAPI with JWT Security Scheme
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        // JWT Configuration
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Nhập JWT token (ví dụ: eyJhbGc...)",
            In = ParameterLocation.Header,
            Name = "Authorization"
        });

        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer", document, null),
                new List<string>()
            }
        });

        // Optional: Add XML comments
        // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        // options.IncludeXmlComments(xmlPath);
    });
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            //Connection path
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            options.LogTo(
            Console.WriteLine,                    // custom logger
            new[] { DbLoggerCategory.Database.Command.Name },  //log SQL commands
            LogLevel.Information                  // Log level
        );
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
    builder.Services.AddHealthChecks().AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "database",
        failureStatus: HealthStatus.Unhealthy,
        timeout: TimeSpan.FromSeconds(5)
    );

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
            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role,
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
        opt.AddPolicy(PolicyConstants.ForAdminOnly, policy => policy.RequireRole(RoleConstants.Admin));
        opt.AddPolicy(PolicyConstants.ForStudent, policy => policy.RequireRole(RoleConstants.Student));
        opt.AddPolicy(PolicyConstants.AuthenticatedUsers, policy => policy.RequireRole(RoleConstants.Admin, RoleConstants.Student, RoleConstants.Teacher));
        opt.AddPolicy(PolicyConstants.TeacherAndAdmin, policy => policy.RequireRole(RoleConstants.Admin, RoleConstants.Teacher));
        //Custom requirement policies
        opt.AddPolicy(PolicyConstants.OnlyUserCanViewUserDetail, policy =>
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
        app.UseSwagger();
        app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "SchoolManagement API v1"));
    }
    app.MapHealthChecks("healthz", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
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
