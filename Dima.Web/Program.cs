using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Dima.Web;
using MudBlazor.Services;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Dima.Core.Handlers;
using Dima.Web.Handlers;
using System.Globalization;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty ;
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddScoped(x => (ICookieAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddMudServices();

builder.Services.AddHttpClient(Configuration.HttpClientName,  option =>
{
    option.BaseAddress = new Uri(Configuration.BackendUrl);
    //option.Timeout = TimeSpan.FromMinutes(5);
}).AddHttpMessageHandler<CookieHandler>();

builder.Services.AddTransient<IAccountHandler, AccountHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<IReportHandler, ReportHandler>();
builder.Services.AddTransient<IEventHandler, EventoHandler>();
builder.Services.AddTransient<IRVSPHandler, RVSPHandler>();

//builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
//CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
//CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

builder.Services.AddLocalization();

// Configura a cultura estática no início da aplicação (antes de qualquer outra coisa)
//var cultureInfo = new CultureInfo("en-US");  // Defina a cultura que você deseja
//CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
//CultureInfo.DefaultThreadCurrentUICulture = cultureInfo

await builder.Build().RunAsync();
