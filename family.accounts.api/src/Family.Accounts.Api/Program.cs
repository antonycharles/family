using System.Text.Json;
using System.Text.Json.Serialization;
using Family.Accounts.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddConfigurationRoot();
var settings = builder.GetSettings();

builder.AddDependence();

builder.Services.AddControllers().AddJsonOptions(o =>
{
o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.AddDataBase(settings);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = settings.RedisConnection;
});

var app = builder.Build();

app.AddMigration();
app.SeedData();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
