using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Tp_Inmobiliaria_Ledesma_Lillo.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=>{
    options.LoginPath = "/Usuarios/Login";
    options.LogoutPath = "/Usuarios/Logout";
    options.AccessDeniedPath = "/Home";
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy =>
     policy.RequireRole("Administrador"));
});
builder.Services.AddTransient<IRepositorio<Propietario>, RepositorioPropietario>();
builder.Services.AddTransient<IRepositorioPropietario, RepositorioPropietario>();
builder.Services.AddTransient<IRepositorioContrato, RepositorioContrato>();
builder.Services.AddTransient<IRepositorioInmueble, RepositorioInmueble>();
builder.Services.AddTransient<IRepositorioInquilino, RepositorioInquilino>();
builder.Services.AddTransient<IRepositorioPago, RepositorioPago>();
builder.Services.AddTransient<IRepositorioTipo, RepositorioTipo>();
builder.Services.AddTransient<IRepositorioUsuario, RepositorioUsuario>();


var app = builder.Build();


//app.UseHttpsRedirection();
app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());
app.UseStaticFiles();


app.UseRouting();
app.UseCookiePolicy(new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.None,
});
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("login", "entrar/{**action}", new {Controller = "Usuario", Action="Login"});

app.Run();
