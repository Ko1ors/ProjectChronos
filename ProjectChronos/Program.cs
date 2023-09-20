using ProjectChronos.DB;
using ProjectChronos.Entities;
using ProjectChronos.Interfaces.Services;
using ProjectChronos.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen.ConventionalRouting;
using System.Net;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IPolygonService, PolygonService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    // These events are called when user is unauthorized and is redirected to login page
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        context.Response.WriteAsync("Forbidden");
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        context.Response.WriteAsync("Forbidden");
        return Task.CompletedTask;
    };
});


builder.Services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
{
    builder.SetIsOriginAllowed(origin => true)
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
}));

builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.Name = ".AspNet.SharedCookie";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Handle all conventional based routes
builder.Services.AddSwaggerGenWithConventionalRoutes(options =>
{
    options.SkipDefaults = true;
});

var app = builder.Build();

app.UseCors("DefaultPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    db.Database.Migrate();
}

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var adminRoleExists = await roleManager.RoleExistsAsync("Administrator");
    if (!adminRoleExists)
    {
        await roleManager.CreateAsync(new IdentityRole("Administrator"));
    }
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "/api/{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(ConventionalRoutingSwaggerGen.UseRoutes);

app.Run();