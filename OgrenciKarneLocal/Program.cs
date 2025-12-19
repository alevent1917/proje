using Microsoft.EntityFrameworkCore;
using OgrenciKarneLocal.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container / Servisleri ekle

// Entity Framework Core ve DbContext yapılandırması
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MVC servislerini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline / HTTP istek pipeline'ını yapılandır
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Varsayılan route yapılandırması
// Student controller'a yönlendirme için route ekle
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}");

app.Run();
