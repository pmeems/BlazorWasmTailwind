using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp.Client;

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
    services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(baseAddress) });

    // services.AddScoped<IFoo, MyFoo>();
}
