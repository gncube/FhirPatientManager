using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FhirPatientManager.Client;
using FhirPatientManager.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register FHIR Service with its own HttpClient
builder.Services.AddHttpClient<IFhirService, FhirService>(client =>
{
    client.BaseAddress = new Uri("https://hapi.fhir.org/baseR4");
    client.Timeout = TimeSpan.FromSeconds(60);
});

await builder.Build().RunAsync();
