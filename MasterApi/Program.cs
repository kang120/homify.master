using MasterDatabase;
using MasterLib;
using MasterLib.Middlewares;
using MasterService.MappingProfiles;
using MasterService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        policy.WithOrigins("https://localhost:3000") // React dev server
              .AllowAnyHeader()                     // Allow headers like Content-Type, Authorization
              .AllowAnyMethod()                     // Allow GET, POST, PUT, DELETE, etc.
              .AllowCredentials();                  // Allow cookies
    });
});

// Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MasterDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

//Newtonsoft.Json.JsonConvert.DefaultSettings = () => new JsonSerializerSettings
//{
//    Formatting = Formatting.None,
//    NullValueHandling = NullValueHandling.Ignore,
//    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
//    DateFormatHandling = DateFormatHandling.IsoDateFormat
//};

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(UserMappingProfile));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BlockService>();
builder.Services.AddScoped<UnitService>();
builder.Services.AddScoped<UserContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("Default");
app.UseMiddleware<ExceptionHandlingMiddleware>();

// AUTO Database Migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
