using Job_Match.Data.Context;
using Job_Match.Services.Interfaces;
using Job_Match.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Job_Match.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMDocTypeService, MDocTypeService>();
builder.Services.AddScoped<IMPostalCodeService, MPostalCodeService>();
builder.Services.AddScoped<IMClientService, MClientService>();
builder.Services.AddScoped<IMProviderService, MProviderService>();
builder.Services.AddScoped<IMServiceTypeService, MServiceTypeService>();
builder.Services.AddScoped<IMServiceService, MServiceService>();
builder.Services.AddScoped<IMServiceProviderService, MServiceProviderService>();
builder.Services.AddScoped<IMTaskRequestService, MTaskRequestService>();
builder.Services.AddScoped<IMTaskStatusService, MTaskStatusService>();
builder.Services.AddScoped<IMOfferSubscriptionService, MOfferSubscriptionService>();
builder.Services.AddScoped<IMChatService, MChatService>();
builder.Services.AddScoped<IMTaskScoreService, MTaskScoreService>();

// Configurar EF + Contexto
builder.Services.AddDbContext<job_match_context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dependencias
builder.Services.AddScoped<IMUserService, MUserService>();

// Configuración JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]))
        };
    });

// Swagger + Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
