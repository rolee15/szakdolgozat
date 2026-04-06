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

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(LoginDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            LoginDto loginDto = await _userService.Login(request.Email, request.Password);
            return Ok(loginDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RegisterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RegisterDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        RegisterDto registerDto = await _userService.Register(request.Email, request.Password);
        if (!registerDto.IsSuccess)
        {
            return BadRequest(registerDto);
        }

        return Ok(registerDto);
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
}
