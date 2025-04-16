using KanjiKa.Api.Services;
using KanjiKa.Core.Interfaces;
using KanjiKa.Core.Services;
using KanjiKa.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IKanaService, KanaService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, DummyEmailService>();

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
builder.Services.AddScoped<KanjiKaDataSeeder>();

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
app.UseAuthorization();
app.MapControllers();

// Seed test data.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<KanjiKaDataSeeder>();
    await seeder.SeedAsync();
}

await app.RunAsync();
