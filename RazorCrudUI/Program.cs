using Data;
using DataMem;
using Microsoft.EntityFrameworkCore;
using RazorCrudUI.Data;
using RazorCrudUI.Models;
using Microsoft.AspNetCore.Identity;
//using UI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ItemContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ItemContext>();

builder.Services.AddScoped<IItemRepository, ItemRepositoryEf>();
//builder.Services.AddScoped<IItemRepository, ItemRepositoryMem>();
//builder.Services.AddSingleton<IItemRepository, ItemRepositoryMem>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
