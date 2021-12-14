using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ReSharper disable once RedundantAssignment
var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

#if DEBUG
    // Set the correct port when running locally:
    baseAddress = new Uri("http://localhost:7071");
#endif

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });

await builder.Build().RunAsync();
