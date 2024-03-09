using System.Runtime.InteropServices;
using eMeterApi.Service;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var _connectionString = builder.Configuration.GetConnectionString("eMeter")!;

// Add services to the container.
builder.Services.AddScoped<EMeterRepository>(provider => new EMeterRepository(_connectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options => 
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.1",
        Title = "EMeter",
        Description = "An .net core web API for storing digital meter measurements.",
        Contact = new OpenApiContact
        {
            Name = "Soluciones Nerus",
            Url = new Uri("http://nerus.com.mx/")
        }
    })
);

// Configure logger
builder.Host.ConfigureLogging( logging => {
    logging.ClearProviders();
    logging.AddConsole();
    
    if( RuntimeInformation.IsOSPlatform( OSPlatform.Windows) ){
        builder.Logging.AddEventLog( eventLogSettings => {
            eventLogSettings.LogName = "Nerus";
            eventLogSettings.SourceName = "eMeter";
        });
    }
});

builder.Services.AddHttpLogging(configureOptions => { 
    configureOptions.LoggingFields = HttpLoggingFields.All;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Logger.LogWarning("Starting the app");
app.Run();
