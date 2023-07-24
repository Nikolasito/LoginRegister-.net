using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Login_Register.Models;

using Login_Register.Servicios.Implementacion;
using Login_Register.Servicios.Contrato;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BdLoginRegisterContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")); //Configuracion de BD
});   

builder.Services.AddScoped<IUsuarioService,UsuarioService>(); //Poder utilizar los servios en los controladores 

//Habilitar la autentificacion
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(Options =>
    {
        Options.LoginPath = "/Inicio/IniciarSesion";
        Options.ExpireTimeSpan = TimeSpan.FromMinutes(15);  //Se cierrra la sesion en 15 min

    });
//Borrar cache de la sesion
builder.Services.AddControllersWithViews(Options =>
{
    Options.Filters.Add(
        new ResponseCacheAttribute
        {
            NoStore = true, 
            Location = ResponseCacheLocation.None,
        }
        );
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

//Usar la autentificacion 
app.UseAuthentication();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

app.Run();
