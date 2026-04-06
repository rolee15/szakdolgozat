using System.Text;
using KanjiKa.Api.Extensions;
using KanjiKa.Application.Interfaces;
using KanjiKa.Application.Services;
using KanjiKa.Data;
using KanjiKa.Data.Repositories;
using KanjiKa.Data.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerConfiguration();
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IKanaService, KanaService>();
builder.Services.AddScoped<IKanaRepository, KanaRepository>();
builder.Services.AddScoped<IKanjiService, KanjiService>();
builder.Services.AddScoped<IKanjiRepository, KanjiRepository>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<IGrammarService, GrammarService>();
builder.Services.AddScoped<IGrammarRepository, GrammarRepository>();
builder.Services.AddScoped<IReadingService, ReadingService>();
builder.Services.AddScoped<IReadingRepository, ReadingRepository>();
builder.Services.AddScoped<IPathRepository, PathRepository>();
builder.Services.AddScoped<IPathService, PathService>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, DummyEmailService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

string? conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<KanjiKaDbContext>(options =>
    options.UseNpgsql(conn));

if (builder.Environment.IsDevelopment())
    builder.Services.AddScoped<IDataSeeder, DevelopmentDataSeeder>();
else
    builder.Services.AddScoped<IDataSeeder, ProductionDataSeeder>();

WebApplication app = builder.Build();

app.UseSwaggerConfiguration();

app.UseCors("AllowReactApp");
// Disabled because the self-signed certificate doesn't work in containers.
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.InitialiseDatabaseAsync();

await app.RunAsync();
