using Microsoft.EntityFrameworkCore;
using store_clothes.Models;
using store_clothes.Models.Momo;
using store_clothes.Services.Momo;

var builder = WebApplication.CreateBuilder(args);

// Đọc chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("storeclothes");

// Đăng ký DbContext với MySQL
builder.Services.AddDbContext<storeclothesContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Thêm dịch vụ session
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();
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
// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<storeclothesContext>();
    context.Database.Migrate(); // Apply any pending migrations

    // Seed categories if they don't exist
    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category { Name = "hoodie" },
            new Category { Name = "jacket" },
            new Category { Name = "polo-shirt" },
            new Category { Name = "shirts" },
            new Category { Name = "t-shirts" }
        );
        context.SaveChanges();
    }
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
    name: "product",
    pattern: "product/{action=ProductAdmin}/{id?}",
    defaults: new { controller = "product" }
);
app.MapControllerRoute(
    name: "product",
    pattern: "product/{action=CreateProduct}/{id?}",
    defaults: new { controller = "product" }
); app.MapControllerRoute(
    name: "product",
    pattern: "product/{action=DeleteProduct}/{id?}",
    defaults: new { controller = "product" }
); app.MapControllerRoute(
    name: "product",
    pattern: "product/{action=UpdateProduct}/{id?}",
    defaults: new { controller = "product" }
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
    name: "checkout",
    pattern: "checkout/{action=PaymentFailed}/{id?}",
    defaults: new { controller = "Checkout" }
);
app.MapControllerRoute(
    name: "payments",
    pattern: "payments/{action=PaymentFailed}/{id?}",
    defaults: new { controller = "Payments" }
)
    ;app.MapControllerRoute(
    name: "payments",
    pattern: "payments/{action=Success}/{id?}",
    defaults: new { controller = "Payments" }
);
app.MapControllerRoute(
    name: "checkout",
    pattern: "checkout/{action=Success}/{id?}",
    defaults: new { controller = "Checkout" }
);
app.MapControllerRoute(
    name: "login",
    pattern: "login/{action=Index}/{id?}",
    defaults: new { controller = "Login" }
);
app.MapControllerRoute(
    name: "login",
    pattern: "login/{action=LoginAdmin}/{id?}",
    defaults: new { controller = "login" }
);
app.MapControllerRoute(
    name: "register",
    pattern: "register/{action=Index}/{id?}",
    defaults: new { controller = "Register" }
);
app.MapControllerRoute(
    name: "Admin",
    pattern: "Admin/{action=Admin}/{id?}",
    defaults: new { controller = "Admin" }
);
app.MapControllerRoute(
    name: "user",
    pattern: "user/{action=Index}/{id?}",
    defaults: new { controller = "User" }
);
app.MapControllerRoute(
    name: "category",
    pattern: "category/{action=Index}/{id?}",
    defaults: new { controller = "category" }
);
app.MapControllerRoute(
    name: "createuser",
    pattern: "user/{action=CreateUser}/{id?}",
    defaults: new { controller = "user" }
);
app.MapControllerRoute(
    name: "updateuser",
    pattern: "user/{action=UpdateUser}/{id?}",
    defaults: new { controller = "user" }
);
app.Run();
