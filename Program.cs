using DogBreedApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register HttpClient + DogApiService
builder.Services.AddHttpClient<DogApiService>();
builder.Services.AddSingleton<DogApiService>();

// Allow MAUI app and any client to call the API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");
app.MapControllers();

// Dynamic port for Render.com, fallback to 10000 locally
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");
