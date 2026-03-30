using System.Security.Claims;
using KanjiKa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/kanji")]
public class KanjiController : ControllerBase
{
    private readonly IKanjiService _kanjiService;

    public KanjiController(IKanjiService kanjiService)
    {
        _kanjiService = kanjiService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("level/{jlptLevel:int}")]
    public async Task<IActionResult> GetByLevel(int jlptLevel)
    {
        if (jlptLevel < 1 || jlptLevel > 5)
            return BadRequest("JLPT level must be between 1 and 5.");

        var result = await _kanjiService.GetKanjiByLevelAsync(jlptLevel, GetUserId());
        return Ok(result);
    }

    [HttpGet("{character}")]
    public async Task<IActionResult> GetDetail(string character)
    {
        var result = await _kanjiService.GetKanjiDetailAsync(character, GetUserId());
        if (result == null) return NotFound();
        return Ok(result);
    }
}
