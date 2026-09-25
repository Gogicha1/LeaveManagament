using LeaveManagament.Data;
using LeaveManagament.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false) // tu true zea mashin avtomaturad ar dallogindeba da localhostidan emails confirm chirdeba
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILeaveTypeServices, LeaveTypeServices>();
builder.Services.AddScoped<ILeaveRequestServices, LeaveRequestServices>();
builder.Services.AddHttpContextAccessor();
if (builder.Environment.IsDevelopment())
{
    // Avoids a 500 on registration/password-reset when no local SMTP server is running.
    builder.Services.AddTransient<IEmailSender, ConsoleEmailSender>();
}
else
{
    builder.Services.AddTransient<IEmailSender, EmailSender>();
}

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages();


app.Run();
