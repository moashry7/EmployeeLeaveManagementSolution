using EmployeeLeaveManagementBLL.Extensions;
using EmployeeLeaveManagementDAL.Extensions;
using EmployeeLeaveManagementWeb.Extensions;

var builder = WebApplication.CreateBuilder(args);



var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDataAccess(connectionString!);
builder.Services.AddBusinessLogic();
builder.Services.AddPresentationServices();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
