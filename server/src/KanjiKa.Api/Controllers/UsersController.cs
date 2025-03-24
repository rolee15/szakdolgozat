using KanjiKa.Core.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var loginDto = await _userService.Login(request.Email, request.Password);
            return Ok(loginDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var registerDto = await _userService.Register(request.Email, request.Password);
        if (!registerDto.isSuccess)
        {
            return BadRequest(registerDto);
        }

        return Ok(registerDto);
    }

    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var forgotPasswordDto = await _userService.ForgotPassword(request.Email);
        return Ok(forgotPasswordDto);
    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var resetPasswordDto = await _userService.ResetPassword(request.Email, request.ResetCode, request.NewPassword);
        if (!resetPasswordDto.isSuccess)
        {
            return BadRequest(resetPasswordDto);
        }

        return Ok(resetPasswordDto);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] string token, string refreshToken)
    {
        var refreshTokenDto = await _userService.RefreshToken(token, refreshToken);
        return Ok(refreshTokenDto);
    }
}