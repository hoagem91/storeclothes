//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using store_clothes.Repository;

var builder = WebApplication.CreateBuilder(args);

// Sử dụng Pomelo.EntityFrameworkCore.MySql
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(ServerVersion.AutoDetect(connectionString))
    )
);

var app = builder.Build();
app.Run();

