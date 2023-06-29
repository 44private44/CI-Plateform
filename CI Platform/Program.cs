
using CIPlatform.Entities.DataModels;
using CIPlatform.Repositories.Repository;
using CIPlatform.Repositories.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CiplatformContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMissionRepository, MissionRepository>();
builder.Services.AddScoped<IMissionDetailsRepository, MissionDetailsRepository>();
builder.Services.AddScoped<IStoryListingRepository, StoryListingRepository>();
builder.Services.AddScoped<IShareStoryRepository, ShareStoryRepository>();
builder.Services.AddScoped<IStoryDetailsRepository, StoryDetailsRepository>();
builder.Services.AddScoped<IUserEditProfileRepository, UserEditProfileRepository>();
builder.Services.AddScoped<ITimesheetRepository, TimesheetRepository>();
builder.Services.AddScoped<IUserAdminRepository, UserAdminRepository>();
builder.Services.AddScoped<ICmsAdminRepository, CmsAdminRepository>();
builder.Services.AddScoped<IMissionAdminRepository, MissionAdminRepository>();
builder.Services.AddScoped<IMissionThemeAdminRepository, MissionThemeAdminRepository>();
builder.Services.AddScoped<IMissionSkillsAdminRepository, MissionSkillsAdminRepository>();
builder.Services.AddScoped<IMissionApplicationRepository, MissionApplicationRepository>();
builder.Services.AddScoped<IStoryAdminRepository, StoryAdminRepository>();
builder.Services.AddScoped<IBannerAdminRepository, BannerAdminRepository>();
builder.Services.AddScoped<ISPMissionDataRepository, SPMissionDataRepository>();

builder.Services.AddSession();


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
    pattern: "{controller=UserAuth}/{action=Login}/{id?}");

app.Run();
