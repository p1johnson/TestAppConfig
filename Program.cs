using Microsoft.Extensions.Configuration;
using Microsoft.Azure.AppConfiguration.AspNetCore;
using Azure.Identity;
using TestAppConfig;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args); 

// Retrieve the endpoint
string endpoint = builder.Configuration.GetValue<string>("Endpoints:AppConfiguration")
    ?? throw new InvalidOperationException("The setting `Endpoints:AppConfiguration` was not found.");

// Load configuration from Azure App Configuration 
// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(new Uri(endpoint), new DefaultAzureCredential())
           // Load all keys that start with `TestApp:` and have no label.
           .Select("TestApp:*", LabelFilter.Null)
           // Reload configuration if any selected key-values have changed.
           .ConfigureRefresh(refreshOptions =>
               refreshOptions.RegisterAll());
});

// Add services to the container.
builder.Services.AddRazorPages();

// Add Azure App Configuration middleware to the container of services.
builder.Services.AddAzureAppConfiguration();

// Bind configuration "TestApp:Settings" section to the Settings object
builder.Services.Configure<Settings>(builder.Configuration.GetSection("TestApp:Settings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use Azure App Configuration middleware for dynamic configuration refresh.
app.UseAzureAppConfiguration();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
