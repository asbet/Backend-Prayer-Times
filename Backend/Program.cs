using Backend;
using Refit;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Backend.Integration.AdhanAPI;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("Firebase/google-service-account.json")
});

var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    })
};

builder.Services
    .AddRefitClient<IPrayerTimesServices>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.aladhan.com"));

builder.Services.AddDbContext<PrayerTimesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PrayerConnection")));


builder.Services.AddScoped<PrayerTimingService>();
builder.Services.AddHostedService<WCheckingTimes>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
