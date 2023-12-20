using System.Text.Json.Serialization;
using _3rd_semester_exam_project.Middleware;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<ILessonRepository, LessonRepository>();
builder.Services.AddSingleton<ISharedRepository, SharedRepository>();
builder.Services.AddSingleton<ICourseRepository, CourseRepository>();
builder.Services.AddSingleton<IAdminRepository, AdminRepository>();
builder.Services.AddSingleton<PasswordHashRepository>();

builder.Services.AddSingleton<LessonService>();
builder.Services.AddSingleton<AdminService>();
builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<CourseService>();
builder.Services.AddSingleton<SharedService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    // Development policy
    options.AddPolicy("AllowAnyOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

    // Production policy
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("https://learning-platform-80506.web.app")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var frontEndRelativePath = "./../frontend/www";
builder.Services.AddSpaStaticFiles(conf => conf.RootPath = frontEndRelativePath);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAnyOrigin");
}
else
{
    app.UseCors("AllowSpecificOrigin");
}
app.UseSecurityHeaders();
app.UseSession();

app.UseHttpsRedirection();

app.UseSpaStaticFiles();

app.UseSpa(conf =>
{
    conf.Options.SourcePath = frontEndRelativePath;
});
app.MapControllers();
app.UseMiddleware<GlobalExceptionHandler>();
app.Run();