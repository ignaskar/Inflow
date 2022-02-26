using Inflow.Shared.Infrastructure;
using Inflow.Shared.Infrastructure.Modules;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureModules();

var assemblies = ModuleLoader.LoadAssemblies(builder.Configuration);
var modules = ModuleLoader.LoadModules(assemblies);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddModularInfrastructure(builder.Configuration, assemblies);
foreach (var module in modules)
{
    module.Register(builder.Services, builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

foreach (var module in modules)
{
    module.Use(app);
}

app.UseHttpsRedirection();

app.UseModularInfrastructure();

app.MapControllers();

assemblies.Clear();
modules.Clear();

app.Run();