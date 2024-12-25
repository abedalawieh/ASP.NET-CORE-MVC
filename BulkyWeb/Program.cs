using Bulky.DataAccess.Data;
using Bulky.DataAccess.DbInitializer;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();


builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("AppConnection")
    ?? throw new Exception(message: "ConnectionString was not loaded")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
builder.Services.AddScoped<IEmailSender, FakeEmailSender>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddAutoMapper(typeof(Program));


builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


string facebookAppSecret =
    Environment.GetEnvironmentVariable("FACEBOOK_APP_SECRET")
    ?? builder.Configuration["FacebookAuth:AppSecret"]
    ?? throw new Exception(message: "FacebookAuth:AppSecret was not loaded");
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = builder.Configuration["FacebookAuth:AppId"] ?? throw new Exception(message: "FacebookAuth:AppId was not loaded");
    options.AppSecret = facebookAppSecret;
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(@$"app-logs\{DateTime.Now:dd-MM/HH-mm--ss}.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.AddSerilog();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

SeedDatabase();

string? spripeKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY") ?? builder.Configuration["Stripe:SecretKey"];
StripeConfiguration.ApiKey = spripeKey ?? throw new Exception(message: "Stripe Secretkey was not loaded");

app.Run();


#region PRIVATE METHODS

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
        dbInitializer?.RunMigrations();

        string adminPassword =
            Environment.GetEnvironmentVariable("ADMIN_PASSWORD")
            ?? builder.Configuration["AdminPassword"]
            ?? throw new Exception(message: "AdminPassword was not loaded");
        dbInitializer?.SeedRoles(adminPassword);
    }
}

#endregion