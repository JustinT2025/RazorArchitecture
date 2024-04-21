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
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ItemContext>();
builder.Services.AddAuthorization(options =>
{
    // in our authorization options we add a policy
    // that requires the user to have the admin role
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("Admin");
    });
});

// add this section to configure options for our razor pages
builder.Services.AddRazorPages(options =>
{
    // secure anything in the Pages/Items folder 
    // by assigning it the admin policy
    // which we created above 
    // saying it requires a user to have the admin role
    options.Conventions.AuthorizeFolder("/Items", "AdminPolicy");
});
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


// add this code
// this will run every time our website starts up
// we need to create a scope here to get access to our ServiceProvider
// which lets us get access to the RoleManager and UserManager
// using ensures everything gets disposed of properly when we are done
using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // check if we already have an admin role
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        // if not make the admin role
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // now we are going to make a default admin user
    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "admin@mostuff.com";
    // DANGER! PASSWORD MUST BE:
    // 6+ chars
    // at least one non alphanumerc character
    // at least one digit ('0'-'9')
    // at least one uppercase ('A'-'Z')
    string password = "Password123!";

    // see if we have already created the user
    // if not create them and give them the admin role
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();
