using ClothX.CustomAttributes;
using ClothX.Data;
using ClothX.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
DbSeeder.connectionString = connectionString;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ClothXDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultUI().AddDefaultTokenProviders();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, ClothXPermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, ClothXPermissionAuthorizationHandler>();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

NLogBuilder.ConfigureNLog("nlog.config");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseStatusCodePagesWithReExecute("/Error/NotFound", "?statusCode={0}");

    app.UseExceptionHandler("/Error/ServerError");
    //app.UseMigrationsEndPoint();
}
else
{

    app.UseStatusCodePagesWithReExecute("/Error/NotFound", "?statusCode={0}");

    app.UseExceptionHandler("/Error/ServerError");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{

    await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
}

app.Run();
