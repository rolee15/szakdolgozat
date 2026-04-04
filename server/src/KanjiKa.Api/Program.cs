using System.Text;
using KanjiKa.Api.Services;
using KanjiKa.Core.Interfaces;
using KanjiKa.Core.Services;
using KanjiKa.Data;
using KanjiKa.Data.Repositories;
using KanjiKa.Data.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

builder.Services.AddScoped<IKanaService, KanaService>();
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
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp",
        x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<KanjiKaDbContext>(options =>
    options.UseNpgsql(conn));

if (builder.Environment.IsDevelopment())
    builder.Services.AddScoped<IDataSeeder, DevelopmentDataSeeder>();
else
    builder.Services.AddScoped<IDataSeeder, ProductionDataSeeder>();

var app = builder.Build();

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

using (var scope = app.Services.CreateScope())
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
