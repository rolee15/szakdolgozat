using System.Security.Claims;
using KanjiKa.Application.DTOs;
using KanjiKa.Application.DTOs.Admin;
using KanjiKa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
    {
        PagedResult<AdminUserDto> result = await _adminService.GetUsersAsync(page, pageSize, search);
        return Ok(result);
    }

    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        AdminUserDetailDto? result = await _adminService.GetUserByIdAsync(id);
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        int adminUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        bool deleted = await _adminService.DeleteUserAsync(adminUserId, id);
        if (!deleted) return BadRequest("Cannot delete this user.");

        return NoContent();
    }
}
