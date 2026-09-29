using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventManager.Web.Data;
using EventManager.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Fallback absoluto para resolução do wwwroot (funciona independente do diretório de VS)
var baseDir = AppContext.BaseDirectory;
while (!string.IsNullOrEmpty(baseDir) && !Directory.Exists(Path.Combine(baseDir, "wwwroot")))
{
    baseDir = Directory.GetParent(baseDir)?.FullName;
}
if (!string.IsNullOrEmpty(baseDir))
{
    var webRoot = Path.Combine(baseDir, "wwwroot");
    builder.Environment.WebRootPath = webRoot;
    builder.Environment.WebRootFileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(webRoot);
    builder.Environment.ContentRootPath = baseDir;
    builder.Environment.ContentRootFileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(baseDir);
}

// Força a porta 5211
builder.WebHost.UseUrls("http://localhost:5211");

// Adiciona serviços ao contêiner.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("String de conexão não encontrada.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // O valor padrão do HSTS é de 30 dias. Para cenários de produção, consulte https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbInitializer.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao criar as Roles no banco de dados.");
    }
}

app.Run();