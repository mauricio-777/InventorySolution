using InventorySystem.Application;
using InventorySystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE SERVICIOS (INYECCIÓN DE DEPENDENCIAS) ---

// Agregamos soporte para Controladores (Las rutas de la API)
builder.Services.AddControllers();

// Enseñamos a la API dónde están tus servicios del Core
// "Scoped" significa que se crea una instancia por cada petición web.
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<StakeholderService>();
builder.Services.AddScoped<StockService>();

// Configuración para Swagger (La página de documentación automática)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. CONFIGURACIÓN DEL PROCESAMIENTO (PIPELINE) ---

// Si estamos desarrollando, mostramos Swagger para probar fácil
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// Activa las rutas que creemos en los controladores
app.MapControllers();

// --- 3. INICIALIZACIÓN DE DB (BOOTSTRAP) ---
// Al igual que en la consola, aseguramos que la DB exista al arrancar.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    // Pedimos los servicios al contenedor
    var userSvc = services.GetRequiredService<UserService>();
    var prodRepo = services.GetRequiredService<ProductRepository>();
    var stkSvc = services.GetRequiredService<StakeholderService>();
    var stockSvc = services.GetRequiredService<StockService>();

    // Creamos tablas si no existen
    userSvc.EnsureTableExists();
    prodRepo.EnsureTableExists();
    stkSvc.EnsureTablesExist();
    stockSvc.EnsureTableExists();
    
    // Creamos Admin por defecto
    userSvc.RegisterUser("admin", "admin123", "Admin", "API_SYSTEM");
}

app.Run();