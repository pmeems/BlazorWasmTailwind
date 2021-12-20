using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp.Client;
using BlazorApp.Client.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ReSharper disable once NotAccessedVariable
// ReSharper disable once RedundantAssignment
var baseAddress = builder.HostEnvironment.BaseAddress;

#if DEBUG
// Set the correct port when running locally:
// ReSharper disable once RedundantAssignment
baseAddress = "http://localhost:7071";
#endif

ConfigureServices(builder.Services, baseAddress);

await builder.Build().RunAsync();

// https://github.com/jsakamoto/BlazorWasmPreRendering.Build
// Extract the service-registration process to the static local function.
static void ConfigureServices(IServiceCollection services, string baseAddress)
{
    services.AddTransient(sp => new HttpClient(new DefaultBrowserOptionsMessageHandler(new HttpClientHandler())
    {
        DefaultBrowserRequestCache = BrowserRequestCache.ForceCache,
        DefaultBrowserRequestMode = BrowserRequestMode.Cors
    })
    {
        BaseAddress = new Uri(baseAddress)
    });

    // Add your own services:
    // services.AddScoped<IFoo, MyFoo>();
}
