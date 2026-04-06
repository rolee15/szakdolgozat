using KanjiKa.Application.DTOs;
using KanjiKa.Application.DTOs.Admin;

namespace KanjiKa.Application.Interfaces;

public interface IAdminService
{
    Task<PagedResult<AdminUserDto>> GetUsersAsync(int page, int pageSize, string? search);

    Task<AdminUserDetailDto?> GetUserByIdAsync(int id);

    Task<bool> DeleteUserAsync(int adminUserId, int targetUserId);
}
