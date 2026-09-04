using EmployeeLeaveManagementDAL.Extensions;
using EmployeeLeaveManagementBLL.Extensions;

var builder = WebApplication.CreateBuilder(args);



var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDataAccess(connectionString!);
builder.Services.AddBusinessLogic();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
