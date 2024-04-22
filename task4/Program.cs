using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using task4_1.Data;
using task4_1.Models;

var builder = WebApplication.CreateBuilder(args);

var dbContext =builder.Services.AddDbContext<AppDbContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .Options;

/*using (var context = new AppDbContext(options))
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}*/

builder.Services.Configure<IdentityOptions>(options =>
{
    // Отключаем базовые ограничения на пароль
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1; // Минимальная длина пароля
    options.Password.RequiredUniqueChars = 0; // Минимальное количество уникальных символов в пароле
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CheckBlockedUserFilter>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();