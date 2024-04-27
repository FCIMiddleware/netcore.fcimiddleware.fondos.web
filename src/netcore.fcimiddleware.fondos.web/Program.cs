using netcore.fcimiddleware.fondos.web.Services.AgColocadores;
using netcore.fcimiddleware.fondos.web.Services.Fondos;
using netcore.fcimiddleware.fondos.web.Services.Monedas;
using netcore.fcimiddleware.fondos.web.Services.Paises;
using netcore.fcimiddleware.fondos.web.Services.Proxies;
using netcore.fcimiddleware.fondos.web.Services.SocDepositarias;
using netcore.fcimiddleware.fondos.web.Services.SocGerentes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// API URLs
builder.Services.Configure<ApiUrls>(
    option => builder.Configuration.GetSection("ApiUrls").Bind(option)
    );

// Proxies
builder.Services.AddHttpClient<ISocDepositariaProxy, SocDepositariaProxy>();
builder.Services.AddHttpClient<IPaisProxy, PaisProxy>();
builder.Services.AddHttpClient<IMonedaProxy, MonedaProxy>();
builder.Services.AddHttpClient<ISocGerenteProxy, SocGerenteProxy>();
builder.Services.AddHttpClient<IAgColocadorProxy, AgColocadorProxy>();
builder.Services.AddHttpClient<IFondoProxy, FondoProxy>();


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
