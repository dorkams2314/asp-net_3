using asp_net_3.Data;
using asp_net_3.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<AdminDeleteService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

#if !DEBUG
app.Use(async (context, next) => {
    if (context.Request.Path.StartsWithSegments("/Admin") ||
        context.Request.Path.StartsWithSegments("/AdminRoles") ||
        context.Request.Path.StartsWithSegments("/AdminUsers") ||
        context.Request.Path.StartsWithSegments("/AdminCategories") ||
        context.Request.Path.StartsWithSegments("/AdminProducts") ||
        context.Request.Path.StartsWithSegments("/AdminCarts") ||
        context.Request.Path.StartsWithSegments("/AdminCartItems") ||
        context.Request.Path.StartsWithSegments("/AdminOrders") ||
        context.Request.Path.StartsWithSegments("/AdminOrderItems")) {
        context.Response.StatusCode = 404;
        return;
    }

    await next();
});
#endif

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Store}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
