using MinimalApplication.Infrastructure.IOC;
using BibliotecaApi.Infrastructure.Middleware;
using Microsoft.OpenApi.Models;
// Ensure the SQLitePCLRaw.bundle_green package is installed in your project.

var builder = WebApplication.CreateBuilder(args);
// Initialize SQLitePCL Batteries using the correct bundle.

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Biblioteca API",
        Version = "v1"
    });
    //c.SwaggerGeneratorOptions = new()
    //{
    //    OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0
    //};
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta maneira: Bearer {seu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca API v1");
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
