using KanjiKa.Core.DTOs;
using KanjiKa.Core.DTOs.Admin;

namespace KanjiKa.Core.Interfaces;

public interface IAdminService
{
    Task<PagedResult<AdminUserDto>> GetUsersAsync(int page, int pageSize, string? search);
    Task<AdminUserDetailDto?> GetUserByIdAsync(int id);
    Task<bool> DeleteUserAsync(int adminUserId, int targetUserId);
}
