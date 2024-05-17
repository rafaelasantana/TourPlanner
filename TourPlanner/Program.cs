global using Microsoft.Toolkit.Mvvm;
global using Microsoft.Toolkit.Mvvm.ComponentModel;
using TourPlanner.Components;
using TourPlanner.Services;
using Radzen;
using TourPlanner.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<TourService>();
builder.Services.AddScoped<TourLogService>();
builder.Services.AddScoped<TimeFormatService>();
builder.Services.AddRadzenComponents();

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000/v1/") });

// Register the HttpClientWrapper
builder.Services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();

// Register your custom API service


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();