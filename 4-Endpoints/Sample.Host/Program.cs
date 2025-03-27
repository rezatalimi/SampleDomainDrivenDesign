using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sample.Configuration;
using Sample.Configuration.Authorizations;
using Sample.Host.HostedServics;
using Sample.Host.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var allowSpecificOrigins = "allowSpecificOrigins";

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddConfigureServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new CamelCaseNamingStrategy()
    };
});

builder.Services.AddHostedService<InitializerHostedService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});

builder.Services.AddSingleton<AccessTheControllers>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
    });
}

app.UseHttpsRedirection();

app.SampleDbContextConfiguration();

app.UseMiddleware<CustomExceptionMiddleware>();

app.UseAuthorization();

app.UseAuthorization();

app.LoadTokens();

app.MapControllers();

app.Run();
