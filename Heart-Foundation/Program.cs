using Heart_Foundation.Helper;
using Heart_Foundation.Helper.Archivos;
using Heart_Foundation.Models;
using Heart_Foundation.Models.seguridad;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


builder.Services.AddIdentity<Usuarios, Roles>(options =>
{
    options.User.RequireUniqueEmail = false;
}).AddEntityFrameworkStores<HeartDBContext>().AddDefaultTokenProviders(); //El mismo principio se aplica si utiliza su propia clase de modelo de rol de aplicación personalizada.

builder.Services.AddControllers();
builder.Services.AddScoped<IFunciones, Funciones>();
builder.Services.AddScoped<IFuncionesContext, FuncionesContext>();
builder.Services.AddScoped<IArchivo, Archivo>();
builder.Services.AddScoped<HeartDBContext>();//Inyección de la dependencia solicitada

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:port")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
//Permite operaciones Sincrónicas en IIS:
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

async Task SeedData()
{
    var scopedFactory = app!.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopedFactory.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<HeartDBContext>();
    var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<Usuarios>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Roles>>();

    await context.Database.EnsureCreatedAsync(); // Usar EnsureCreatedAsync si estás en un contexto asincrónico

    if (!usermanager.Users.Any())
    {

        var newuser = new Usuarios
        {
            Email = "admin@prueba.com",
            UserName = "Administrador",
            NombreCompleto = "Administrador",
            FechaCreacion = DateTime.UtcNow,
            FechaModificacion = DateTime.UtcNow,
            Estado = true,
            Version = 1
        };
        var s = await usermanager.CreateAsync(newuser, "Admin123$");

        if (!roleManager.Roles.Any())
        {
            var listRols = new List<string>() { "Admin","Natural","Representante"};
            foreach (var role in listRols)
            {
                var rol = new Roles
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = role,
                    FechaCreacion = DateTime.UtcNow,
                    FechaModificacion = DateTime.UtcNow
                };
                await roleManager.CreateAsync(rol); // Esperar la finalización de la creación del rol
            }
           
        }
        if (s.Succeeded)
        {
            await usermanager.AddClaimAsync(newuser, new Claim("Id", newuser.Id));
            await usermanager.AddClaimAsync(newuser, new Claim("UserName", newuser.UserName));
            await usermanager.AddToRoleAsync(newuser, "Admin"); // Esperar la finalización de agregar usuario al rol
        }

    }

}

await SeedData();

app.Run();
