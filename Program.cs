using eMeterApi.Service;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



var _connectionString = builder.Configuration.GetConnectionString("eMeter");

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
