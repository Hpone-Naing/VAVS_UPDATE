global using System.ComponentModel.DataAnnotations.Schema;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.EntityFrameworkCore;
global using VAVS_Client.Services;
global using VAVS_Client.Services.Impl;
global using VAVS_Client.Models;

using VAVS_Client.Data;
using VAVS_Client.Factories;
using VAVS_Client.Factories.Impl;
using FireSharp.Config;
using FireSharp.Interfaces;
using VAVS_Client.APIFactory;
using VAVS_Client.APIFactory.Impl;
using Microsoft.AspNetCore.DataProtection;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IFirebaseConfig>(provider =>
{
    return new FirebaseConfig
    {
        AuthSecret = "MWhnm4SP7v7w2JWXMKjw9nN8ZSVhiMeSpHku4v5T",
        BasePath = "https://dis-vavs-default-rtdb.firebaseio.com/"
    };
});

builder.Services.AddDbContext<VAVSClientDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VAVSClientDBContext"), sqlServerOptionsAction: SqlOptions => { SqlOptions.EnableRetryOnFailure(); }));

builder.Services.AddTransient<ServiceFactory, ServiceFactoryImpl>();
builder.Services.AddTransient<FileService, FileServiceImpl>();
builder.Services.AddTransient<APIServiceFactory, APIServiceFactoryImpl>();
builder.Services.AddSingleton<SessionService, SessionServiceImpl>();
builder.Services.AddSingleton<FinancialYearService, FinancialYearServiceImpl>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}");

app.Run();

