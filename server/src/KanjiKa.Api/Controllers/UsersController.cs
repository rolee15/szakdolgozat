using System.Security.Claims;
using KanjiKa.Application.DTOs.User;
using KanjiKa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserSettingsService _userSettingsService;

    public UsersController(IUserService userService, IUserSettingsService userSettingsService)
    {
        _userService = userService;
        _userSettingsService = userSettingsService;
    }

    [HttpPost("login")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(LoginDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        LoginDto loginDto = await _userService.Login(request.Email, request.Password);
        return Ok(loginDto);
    }

    [HttpPost("register")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RegisterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RegisterDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        RegisterDto registerDto = await _userService.Register(request.Email, request.Password);
        if (!registerDto.Success)
        {
            return BadRequest(registerDto);
        }

        return Ok(registerDto);
    }

    [AllowAnonymous]
    [HttpPost("activate")]
    [ProducesResponseType(typeof(ActivateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ActivateDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Activate([FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return BadRequest(new ActivateDto(false, "Token is required."));
        var result = await _userService.ActivateAccount(token);
        return Ok(result);
    }

    [HttpPost("forgotPassword")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ForgotPasswordDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        ForgotPasswordDto forgotPasswordDto = await _userService.ForgotPassword(request.Email);
        return Ok(forgotPasswordDto);
    }

    [HttpPost("resetPassword")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ResetPasswordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResetPasswordDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        ResetPasswordDto resetPasswordDto = await _userService.ResetPassword(request.Email, request.ResetCode, request.NewPassword);
        if (!resetPasswordDto.IsSuccess)
        {
            return BadRequest(resetPasswordDto);
        }

        return Ok(resetPasswordDto);
    }

    [HttpPost("refreshToken")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RefreshTokenDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] string token, string refreshToken)
    {
        RefreshTokenDto refreshTokenDto = await _userService.RefreshToken(token, refreshToken);
        return Ok(refreshTokenDto);
    }

    [Authorize]
    [HttpPost("changePassword")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ChangePasswordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ChangePasswordDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        ChangePasswordDto result = await _userService.ChangePassword(userId, request.CurrentPassword, request.NewPassword);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize]
    [HttpGet("settings")]
    [ProducesResponseType(typeof(UserSettingsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSettings()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        UserSettingsDto settings = await _userSettingsService.GetSettingsAsync(userId);
        return Ok(settings);
    }

    [Authorize]
    [HttpPut("settings")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSettings([FromBody] UpdateUserSettingsDto dto)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _userSettingsService.UpdateSettingsAsync(userId, dto);
        return NoContent();
    }
}
