using Front_Auth1.Services; // Aseg�rate de tener este using

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Configuraci�n de la Sesi�n (para guardar el token)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// 2. Registrar el Servicio de Autenticaci�n con HttpClient
builder.Services.AddHttpClient<AuthService>(client =>
{
    // *** USAMOS EL PUERTO HTTPS 7036 DE TU API ***
    client.BaseAddress = new Uri("https://localhost:7036/");
});
// Registrar el servicio de autenticaci�n
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3. Agregar el Middleware de Sesi�n (DEBE ir antes de UseAuthorization)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
