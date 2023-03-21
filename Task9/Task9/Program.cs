using Microsoft.EntityFrameworkCore;
using Task9.Data;
using Task9.Models;
using Task9.Services;
using Task9.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Task9Context>(options => options.UseSqlServer(connection));
builder.Services.AddTransient<ICRUDService<Course>, CourseService>();
builder.Services.AddTransient<ICRUDService<Group>, GroupService>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddTransient<ICRUDService<Student>, StudentService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
