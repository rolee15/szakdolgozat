using System.Security.Claims;
using KanjiKa.Core.DTOs.Kanji;
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

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int? jlptLevel,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 200) pageSize = 50;
        var result = await _kanjiService.GetKanjiPagedAsync(jlptLevel, page, pageSize, GetUserId());
        return Ok(result);
    }

    [HttpGet("reviews/count")]
    public async Task<IActionResult> GetDueReviewsCount()
    {
        var count = await _kanjiService.GetDueReviewsCountAsync(GetUserId());
        return Ok(new { count });
    }

    [HttpGet("reviews")]
    public async Task<IActionResult> GetDueReviews()
    {
        var reviews = await _kanjiService.GetDueReviewsAsync(GetUserId());
        return Ok(reviews);
    }

    [HttpPost("reviews/check")]
    public async Task<IActionResult> CheckReview([FromBody] KanjiReviewAnswerDto answer)
    {
        try
        {
            var result = await _kanjiService.CheckReviewAsync(GetUserId(), answer);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("learn/{kanjiId:int}")]
    public async Task<IActionResult> LearnKanji(int kanjiId)
    {
        try
        {
            await _kanjiService.LearnKanjiAsync(GetUserId(), kanjiId);
            return Ok();
        }
        catch (InvalidOperationException)
        {
            return BadRequest("Kanji has already been learned.");
        }
    }
}
