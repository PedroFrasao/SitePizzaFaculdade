using Microsoft.EntityFrameworkCore;
using SitePizzaFaculdade.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzariaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicione essa linha para registrar IHttpClientFactory
builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
