using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniJiraApp.Data;
using MiniJiraApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- INICIO DE LA RESOLUCIÓN DE IP PARA WSL ---
static string? ResolveWslHost()
{
    try
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "sh",
                Arguments = "-c \"ip route | grep default | awk '{print $3}'\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        string output = process.StandardOutput.ReadToEnd().Trim();
        process.WaitForExit();
        return string.IsNullOrWhiteSpace(output) ? null : output;
    }
    catch { return null; }
}

var baseConnection = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost,1433;Database=MiniJiraDb;User Id=none;Password=awl;TrustServerCertificate=True;";

var csb = new SqlConnectionStringBuilder(baseConnection);
var basePort = "1433";
var dataSourceParts = csb.DataSource.Split(',');
if (dataSourceParts.Length > 1) basePort = dataSourceParts[^1].Trim();

var hostOverride = builder.Configuration["Database:HostOverride"];
var autoResolve = builder.Configuration.GetValue("Database:AutoResolveWslHost", true);

string resolvedHost;
if (!string.IsNullOrWhiteSpace(hostOverride))
{
    resolvedHost = hostOverride.Trim();
}
else if (autoResolve && ResolveWslHost() is string wslIp)
{
    resolvedHost = wslIp;
}
else
{
    resolvedHost = (dataSourceParts.Length > 0 ? dataSourceParts[0] : "localhost");
}

csb.DataSource = $"{resolvedHost},{basePort}";
var finalConnectionString = csb.ConnectionString;
Console.WriteLine($"🔌 SQL Server conectado a través de: {csb.DataSource}");
// --- FIN DE TU RESOLUCIÓN DE IP PARA WSL ---

// Registrar el DbContext con la cadena de conexión final calculada.
// Se habilitan reintentos automáticos para tolerar latencias de SQL Server en WSL.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(finalConnectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
        sqlOptions.CommandTimeout(60);
    }));

// Configurar .NET Identity básico
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configurar la redirección si un usuario no está autenticado
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

// Registro de los repositorios (Patrón Repositorio + Inyección de Dependencias)
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();

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

// Asegurar el pipeline de autenticación
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();