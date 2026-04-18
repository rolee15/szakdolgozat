using KanjiKa.Application.DTOs;
using KanjiKa.Application.DTOs.Admin;
using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Application.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _repo;

    public AdminService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<PagedResult<AdminUserDto>> GetUsersAsync(int page, int pageSize, string? search)
    {
        (List<UserSummary> users, int totalCount) = await _repo.GetUsersPagedAsync(page, pageSize, search);

        return new PagedResult<AdminUserDto>
        {
            Items = users.Select(u => new AdminUserDto
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role,
                MustChangePassword = u.MustChangePassword,
                ProficiencyCount = u.ProficiencyCount,
                LessonCompletionCount = u.LessonCompletionCount
            }).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<AdminUserDetailDto?> GetUserByIdAsync(int id)
    {
        User? user = await _repo.GetByIdWithStatsAsync(id);
        if (user == null) return null;

        return new AdminUserDetailDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            MustChangePassword = user.MustChangePassword,
            ProficiencyCount = user.KanaProficiencies.Count,
            LessonCompletionCount = user.LessonCompletions.Count,
            Proficiencies = user.KanaProficiencies.Select(p => new ProficiencySummaryDto
            {
                CharacterId = p.CharacterId,
                CharacterSymbol = p.Character?.Symbol ?? "",
                LearnedAt = p.LearnedAt,
                LastPracticed = p.LastPracticedAt
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
