using MinimalApplication.Infrastructure.IOC;
// Ensure the SQLitePCLRaw.bundle_green package is installed in your project.

var builder = WebApplication.CreateBuilder(args);
// Initialize SQLitePCL Batteries using the correct bundle.

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Biblioteca API",
        Version = "v1"
    });
    //c.SwaggerGeneratorOptions = new()
    //{
    //    OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0
    //};
}); ;

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
