using DoAn_LTWeb.Data;
using DoAn_LTWeb.Hubs;
using DoAn_LTWeb.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using static DoAn_LTWeb.Repositories.EmailSender;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Đăng ký Repository
builder.Services.AddScoped<IAdminRepository, KhachHangRepository>();
builder.Services.AddScoped<IVehicalRepository, EFVehicalRepository>();
builder.Services.AddScoped<INhaXeRepository, EFNhaXeRepository>();

builder.Services.AddScoped<IPhongTroRepository, EFPhongTroRepository>();
builder.Services.AddScoped<INhaTroRepository, EFNhaTroRepository>();
builder.Services.AddScoped<INoiThatRepository, EFNoiThatRepository>();
builder.Services.AddScoped<IChiTietPhongTroRepository, EFChiTietPhongTroRepository>();
builder.Services.AddScoped<INhanVienRepository, EFNhanVienRepository>();
builder.Services.AddScoped<IHoaDonRepository, EFHoaDonRepository>();
builder.Services.AddScoped<IDieuKhoanRepository, EFDieuKhoanRepository>();
builder.Services.AddScoped<IHopDongRepository, EFHopDongRepository>();
builder.Services.AddScoped<IChiTietThuePhongRepository, EFChiTietThuePhongRepository>();
builder.Services.AddScoped<EFAspRepository>();

builder.Services.AddSignalR();
// Thêm session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Thời gian lưu session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSession();

// Cấu hình Identity có Role-based Authorization
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Định tuyến Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Kéo dài thời gian đăng nhập
    options.SlidingExpiration = true;
});

// Setting về email sender
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();



builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:7264") // Đổi theo URL frontend của bạn
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Tạo Role mặc định khi chạy ứng dụng
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    await CreateRoles(roleManager, userManager);
}


app.UseCors(MyAllowSpecificOrigins);

// Hàm tạo Role
async Task CreateRoles(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Tạo Admin mặc định (nếu chưa có)
    string adminEmail = "admin@example.com";
    string adminPassword = "Ad@123";
    string userEmail = "user@example.com";
    string userPassword = "Ad@123";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        else
        {
            Console.WriteLine($"Lỗi khi tạo Admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
    var User = await userManager.FindByEmailAsync(userEmail);
    if (User == null)
    {
        User = new IdentityUser { UserName = userEmail, Email = userEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(User, userPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(User, "User");
        }
        else
        {
            Console.WriteLine($"Lỗi khi tạo User: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    //Route cho các Areas
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller}/{action}/{id?}"
    );

    // Route mặc định không có Area
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapHub<BookingHub>("/bookingHub"); // Định tuyến SignalR
});



app.MapRazorPages();

app.Run();



