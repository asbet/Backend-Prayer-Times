using Backend;
using Refit;
using System.Text.Json;
using Backend.Configuration;
using Microsoft.EntityFrameworkCore;
using Backend.Integration.AdhanAPI;
using Backend.Notification;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
// Configure Kestrel with fixed ports
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5038); // HTTP
    options.ListenAnyIP(5039, listenOptions => // HTTPS
    {
        listenOptions.UseHttps(); // Uses development certificate
    });
});
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


// Register FCMNotification as a service
builder.Services.AddScoped<FCMNotification>();


//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(5038); // HTTP
//    options.ListenAnyIP(44383, listenOptions =>
//    {
//        listenOptions.UseHttps(); // Enable HTTPS
//    });
//});


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

builder.Services.AddScoped<TestService>();
builder.Services.AddHostedService<PrayerNotificationService>();

builder.Services.AddScoped<CheckExistDatas>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; // Optional: for pretty JSON
    });
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10)); // Important for POST
    });
});
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutter",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowFlutter");



app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();