using Microsoft.EntityFrameworkCore;
using store_clothes.Models;

var builder = WebApplication.CreateBuilder(args);

// Đọc chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("storeclothes");

// Đăng ký DbContext với MySQL
builder.Services.AddDbContext<storeclothesContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Thêm dịch vụ session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseStaticFiles(); // Hỗ trợ file tĩnh (CSS, JS, Images)

// Thêm middleware session
app.UseSession();

app.UseRouting(); // Cấu hình định tuyến
app.UseAuthorization(); // Hỗ trợ xác thực người dùng


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
