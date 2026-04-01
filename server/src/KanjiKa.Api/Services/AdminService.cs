using KanjiKa.Core.DTOs;
using KanjiKa.Core.DTOs.Admin;
using KanjiKa.Core.Interfaces;

namespace KanjiKa.Api.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _repo;

    public AdminService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<PagedResult<AdminUserDto>> GetUsersAsync(int page, int pageSize, string? search)
    {
        var (users, totalCount) = await _repo.GetUsersPagedAsync(page, pageSize, search);

        return new PagedResult<AdminUserDto>
        {
            Items = users.Select(u => new AdminUserDto
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role,
                MustChangePassword = u.MustChangePassword,
                ProficiencyCount = u.Proficiencies.Count,
                LessonCompletionCount = u.LessonCompletions.Count
            }).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<AdminUserDetailDto?> GetUserByIdAsync(int id)
    {
        var user = await _repo.GetByIdWithStatsAsync(id);
        if (user == null) return null;

        return new AdminUserDetailDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            MustChangePassword = user.MustChangePassword,
            ProficiencyCount = user.Proficiencies.Count,
            LessonCompletionCount = user.LessonCompletions.Count,
            Proficiencies = user.Proficiencies.Select(p => new ProficiencySummaryDto
            {
                CharacterId = p.CharacterId,
                CharacterSymbol = p.Character?.Symbol ?? "",
                LearnedAt = p.LearnedAt,
                LastPracticed = p.LastPracticed
            }).ToList(),
            LessonCompletions = user.LessonCompletions.Select(lc => new LessonCompletionSummaryDto
            {
                CharacterId = lc.CharacterId,
                CharacterSymbol = lc.Character?.Symbol ?? "",
                CompletionDate = lc.CompletionDate
            }).ToList()
        };
    }

    public async Task<bool> DeleteUserAsync(int adminUserId, int targetUserId)
    {
        if (adminUserId == targetUserId)
            return false;

        return await _repo.DeleteByIdAsync(targetUserId);
    }
}
