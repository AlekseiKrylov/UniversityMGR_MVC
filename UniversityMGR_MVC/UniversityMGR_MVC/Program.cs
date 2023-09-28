using Microsoft.EntityFrameworkCore;
using UniversityMGR_MVC.Data;
using UniversityMGR_MVC.Models;
using UniversityMGR_MVC.Services;
using UniversityMGR_MVC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UniversityMGRContext>(options => options.UseSqlServer(connection));
builder.Services.AddTransient<ICRUDService<Course>, CourseService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<ICRUDService<Group>, GroupService>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddTransient<ICRUDService<Student>, StudentService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UniversityMGRContext>();
    dbContext.Database.Migrate();
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
