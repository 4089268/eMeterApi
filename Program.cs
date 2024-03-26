using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using eMeterApi.Data;
using eMeterApi.Data.Contracts;
using eMeterApi.Service;
using eMeter.Service;

var builder = WebApplication.CreateBuilder(args);

var _connectionString = builder.Configuration.GetConnectionString("eMeter")!;

// Add services to the container.
builder.Services.AddAuthentication( o => {
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( x => {
    x.TokenValidationParameters = new TokenValidationParameters{
        ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes( builder.Configuration.GetValue<string>("JwtSettings:Key") )
        ),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession( options => {
    options.IdleTimeout = TimeSpan.FromHours(6);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<EMeterRepository>(provider => new EMeterRepository(_connectionString));
builder.Services.AddDbContext<EMeterContext>( o => {
    o.UseSqlServer( builder.Configuration.GetConnectionString(_connectionString) );
});
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<MeasurementService>();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.Configure<JwtSettings>( builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<AppSettings>( o => o.AppKey = builder.Configuration.GetValue<string>("AppKey"));

// builder.Services.AddEndpointsApiExplorer();
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
app.UseSwagger();
app.UseSwaggerUI( o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "eMeter API V1") );
app.UseSession();

app.UseHttpLogging();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.Run();