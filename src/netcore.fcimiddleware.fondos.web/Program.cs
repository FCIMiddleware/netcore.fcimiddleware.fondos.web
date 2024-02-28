using Microsoft.Extensions.FileProviders;
using netcore.fcimiddleware.fondos.web.Services.Moneda;
using netcore.fcimiddleware.fondos.web.Services.Proxies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// API URLs
builder.Services.Configure<ApiUrls>(
    option => builder.Configuration.GetSection("ApiUrls").Bind(option)
    );

// Proxies
builder.Services.AddHttpClient<IMonedaProxy, MonedaProxy>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
