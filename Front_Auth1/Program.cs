using Front_Auth1.Services;
using Front_Auth1.Controllers; // Necesario para UsuariosService
using System;
using Microsoft.AspNetCore.Http; // Necesario para IHttpContextAccessor

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- INYECCIÓN DE DEPENDENCIAS Y CONFIGURACIÓN ---

// 1. Configuración de la Sesión (para guardar el token)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 2. Registrar el IHttpContextAccessor (ESENCIAL para leer la sesión en ApiService)
builder.Services.AddHttpContextAccessor(); // <--- ¡NUEVO!

// 3. Obtener la URL base de configuración
var apiBaseUrl = builder.Configuration.GetValue<string>("ServiceUrls:APIBase") ?? "https://localhost:7036/";

// 4. Registrar los servicios con HttpClient (para que reciban el HttpClient en el constructor)

// A. Registrar AuthService: Usado para Login/Registro. Ahora el constructor recibirá HttpClient y IHttpContextAccessor
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// B. Registrar UsuariosService: Usado para la lista protegida de usuarios.
// También necesita HttpClient y IHttpContextAccessor (vía ApiService)
builder.Services.AddHttpClient<UsuariosService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

// --- PIPELINE DE MIDDLEWARES ---

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 5. Agregar el Middleware de Sesión (DEBE ir ANTES de UseAuthorization)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();