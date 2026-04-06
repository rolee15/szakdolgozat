using System.Text;
using KanjiKa.Api.Filters;
using KanjiKa.Application.Interfaces;
using KanjiKa.Application.Services;
using KanjiKa.Data;
using KanjiKa.Data.Repositories;
using KanjiKa.Data.Seeders;
using KanjiKa.Domain.Interfaces;
using KanjiKa.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KanjiKa API",
        Version = "v1",
        Description = "REST API for the KanjiKa Japanese Hiragana/Katakana learning platform."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token (without the 'Bearer ' prefix)."
    });

    options.OperationFilter<AuthorizeOperationFilter>();
});
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
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

builder.Services.AddScoped<IKanaService, KanaService>();
builder.Services.AddScoped<IKanaRepository, KanaRepository>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, DummyEmailService>();
builder.Services.AddScoped<IKanjiRepository, KanjiRepository>();
builder.Services.AddScoped<IKanjiService, KanjiService>();
builder.Services.AddScoped<IGrammarRepository, GrammarRepository>();
builder.Services.AddScoped<IGrammarService, GrammarService>();
builder.Services.AddScoped<IReadingRepository, ReadingRepository>();
builder.Services.AddScoped<IReadingService, ReadingService>();
builder.Services.AddScoped<IPathRepository, PathRepository>();
builder.Services.AddScoped<IPathService, PathService>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp",
        configurePolicy: x => x
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
// Disabled because the self-signed certificate doesn't work in containers.
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<KanjiKaDbContext>();

    if (app.Environment.IsDevelopment())
    {
        // Terminate other connections to avoid EnsureDeletedAsync timeout
        try
        {
            await db.Database.ExecuteSqlRawAsync(
                "SELECT pg_terminate_backend(pg_stat_activity.pid) " +
                "FROM pg_stat_activity " +
                "WHERE pg_stat_activity.datname = current_database() " +
                "AND pid <> pg_backend_pid();");
        }
        catch
        {
            // Database may not exist yet on first run
        }

        await db.Database.EnsureDeletedAsync();
    }

    await db.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedAsync();
}

await app.RunAsync();
