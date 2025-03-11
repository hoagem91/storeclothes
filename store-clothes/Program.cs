using Microsoft.EntityFrameworkCore;
using store_clothes.Models;

var builder = WebApplication.CreateBuilder(args);

// Đọc chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("storeclothes");

// Đăng ký DbContext với MySQL
builder.Services.AddDbContext<storeclothesContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Đăng ký dịch vụ MVC
builder.Services.AddControllersWithViews();

// ✅ Đăng ký Session
builder.Services.AddSession();

var app = builder.Build();

// Middleware xử lý lỗi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// ✅ Kích hoạt Session Middleware
app.UseSession();

app.UseAuthorization();

// ✅ Đặt MainController làm trang mặc định thay vì HomeController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}"
);

// Giữ HomeController và các route khác
app.MapControllerRoute(
    name: "home",
    pattern: "home/{action=Index}/{id?}",
    defaults: new { controller = "Home" }
);

app.MapControllerRoute(
    name: "main",
    pattern: "main/{action=Index}/{id?}",
    defaults: new { controller = "Main" }
);

app.MapControllerRoute(
    name: "product",
    pattern: "product/{action=Index}/{id?}",
    defaults: new { controller = "Product" }
);

app.MapControllerRoute(
    name: "cart",
    pattern: "cart/{action=Index}/{id?}",
    defaults: new { controller = "Cart" }
);

app.MapControllerRoute(
    name: "payments",
    pattern: "payments/{action=Index}/{id?}",
    defaults: new { controller = "Payments" }
);

app.MapControllerRoute(
    name: "login",
    pattern: "login/{action=Index}/{id?}",
    defaults: new { controller = "Login" }
);

app.MapControllerRoute(
    name: "register",
    pattern: "register/{action=Index}/{id?}",
    defaults: new { controller = "Register" }
);

app.Run();
