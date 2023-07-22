using Microsoft.Data.Sqlite;
using WorkshopApp.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntityFrameworkSqlite().AddDbContext<StoreDBContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

IHostApplicationLifetime lifetime = app.Lifetime;
lifetime.ApplicationStopped.Register(() =>
{
    SqliteConnection.ClearAllPools();
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
