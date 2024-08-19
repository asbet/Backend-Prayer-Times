using Backend;
using Refit;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    })
};


builder.Services
    .AddRefitClient<PrayerTimesServices>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://api.aladhan.com"));
builder.Services.AddDbContext<PrayerTimesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PrayerConnection")));



var app = builder.Build();


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
