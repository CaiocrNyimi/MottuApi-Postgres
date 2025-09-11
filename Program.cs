using Microsoft.EntityFrameworkCore;
using MottuApi.Data;

var builder = WebApplication.CreateBuilder(args);
var MyAllowAnyOriginPolicy = "_myAllowAnyOriginPolicy";

// Adiciona o DbContext com a string de conexão do Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Adiciona suporte a controllers (ESSENCIAL para o Swagger mostrar seus endpoints!)
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

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

app.UseHttpsRedirection();

// Ativa os endpoints dos controllers
app.MapControllers();

app.Run();