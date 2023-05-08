
using CI_Platform.Entities.Data;
using CI_Platform.Repository.Repository;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(180);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CiPlatformContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("Data Source=PCI14\\SQL2017;DataBase=Ci_Platform;User ID=sa;Password=Tatva@123;Encrypt=False")));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IThemeRepository, ThemeRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IMissionRepository, MissionRepository>();
builder.Services.AddScoped<IMissiondataRepository, MissiondataRepository>();
builder.Services.AddScoped<IVolunteerRepository, VolunteerRepository>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();


app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


// Add services to the container.
builder.Services.AddControllersWithViews();



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Forgot}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Registration}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Reset}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mission}/{action=Landing}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mission}/{action=Volunteering}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Story}/{action=Story}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Story}/{action=Storylisting}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Story}/{action=Storydetail}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Profile}/{action=UserProfile}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Policy}/{action=PolicyPage}/{id?}");

app.UseSession();

app.Run();
