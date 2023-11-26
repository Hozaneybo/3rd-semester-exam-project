using _3rd_semester_exam_project.Middleware;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Service;
using Service.AdminService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
builder.Services.AddSingleton<AccountRepository>();
builder.Services.AddSingleton<ILessonRepository, LessonRepository>();
builder.Services.AddSingleton<LessonService>();
builder.Services.AddSingleton<PasswordHashRepository>();
builder.Services.AddSingleton<AdminRepository>();
builder.Services.AddSingleton<AdminService>();
builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<CourseService>();
builder.Services.AddSingleton<ICourseRepository, CourseRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSecurityHeaders();
app.UseSession();

app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<GlobalExceptionHandler>();
app.Run();