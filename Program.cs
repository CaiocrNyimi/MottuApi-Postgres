using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Examples;
using MottuApi.Services.Interfaces;
using MottuApi.Services.Implementations;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var MyAllowAnyOriginPolicy = "_myAllowAnyOriginPolicy";

// Implementando Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("A API está rodando perfeitamente!"));

// Conexão com banco Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Injeção de dependência para serviços
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IPatioService, PatioService>();
builder.Services.AddScoped<IMovimentacaoService, MovimentacaoService>();

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    options.ExampleFilters();
});

// Registra exemplos dos modelos
builder.Services.AddSwaggerExamplesFromAssemblyOf<MotoRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<MovimentacaoRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<PatioRequestExample>();
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowAnyOriginPolicy,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

// Pipeline de requisição
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Mapeando /health para Health Check
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
})
.AllowAnonymous();

app.UseHttpsRedirection();
app.UseCors(MyAllowAnyOriginPolicy);
app.MapControllers();

app.Run();