using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT authentication
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Define API endpoints
app.MapPost("/register", async (AppDbContext db, UserRegistrationDto userDto) =>
{
    if (await db.Users.AnyAsync(u => u.Email == userDto.Email))
    {
        return Results.BadRequest("Email already registered");
    }

    var user = new User
    {
        Email = userDto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok("User registered successfully");
});

app.MapPost("/login", async (AppDbContext db, UserLoginDto userDto) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
    if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
    {
        return Results.Unauthorized();
    }

    var token = GenerateJwtToken(user);
    return Results.Ok(new { Token = token });
});

app.MapPost("/forgot-password", async (AppDbContext db, ForgotPasswordDto forgotPasswordDto) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);
    if (user == null)
    {
        // Don't reveal that the user doesn't exist
        return Results.Ok("If the email exists, a reset link will be sent.");
    }

    // Generate password reset token
    var resetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    user.ResetToken = resetToken;
    user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);
    await db.SaveChangesAsync();

    // Send email with reset token (implement actual email sending logic)
    // For demo purposes, we'll just return the token
    return Results.Ok(new { ResetToken = resetToken });
});

app.MapPost("/reset-password", async (AppDbContext db, ResetPasswordDto resetPasswordDto) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.ResetToken == resetPasswordDto.ResetToken);
    if (user == null || user.ResetTokenExpires < DateTime.UtcNow)
    {
        return Results.BadRequest("Invalid or expired reset token");
    }

    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
    user.ResetToken = null;
    user.ResetTokenExpires = null;
    await db.SaveChangesAsync();

    return Results.Ok("Password reset successfully");
});

await app.RunAsync();


// Helper method to generate JWT token
static string GenerateJwtToken(User user)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes("YourSecretKeyHere"); // Use a secure key and store it properly
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}

// Database context
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}

// Models
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string ResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
}

// DTOs
public class UserRegistrationDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}

public class UserLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

public class ResetPasswordDto
{
    [Required]
    public string ResetToken { get; set; }

    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; }
}