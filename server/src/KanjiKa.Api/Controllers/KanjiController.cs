using System.Security.Claims;
using KanjiKa.Application.DTOs;
using KanjiKa.Application.DTOs.Kanji;
using KanjiKa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/kanji")]
[Produces("application/json")]
public class KanjiController : ControllerBase
{
    private readonly IKanjiService _kanjiService;

    public KanjiController(IKanjiService kanjiService)
    {
        _kanjiService = kanjiService;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet("level/{jlptLevel:int}")]
    [ProducesResponseType(typeof(List<KanjiDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByLevel(int jlptLevel)
    {
        if (jlptLevel < 1 || jlptLevel > 5)
            return BadRequest("JLPT level must be between 1 and 5.");

        List<KanjiDto> result = await _kanjiService.GetKanjiByLevelAsync(jlptLevel, GetUserId());
        return Ok(result);
    }

    [HttpGet("{character}")]
    [ProducesResponseType(typeof(KanjiDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetail(string character)
    {
        KanjiDetailDto? result = await _kanjiService.GetKanjiDetailAsync(character, GetUserId());
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<KanjiDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int? jlptLevel,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 200) pageSize = 50;
        PagedResult<KanjiDto> result = await _kanjiService.GetKanjiPagedAsync(jlptLevel, page, pageSize, GetUserId());
        return Ok(result);
    }

    [HttpGet("reviews/count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDueReviewsCount()
    {
        int count = await _kanjiService.GetDueReviewsCountAsync(GetUserId());
        return Ok(new { count });
    }

    [HttpGet("reviews")]
    [ProducesResponseType(typeof(List<KanjiReviewDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDueReviews()
    {
        List<KanjiReviewDto> reviews = await _kanjiService.GetDueReviewsAsync(GetUserId());
        return Ok(reviews);
    }

    [HttpPost("reviews/check")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(KanjiReviewResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckReview([FromBody] KanjiReviewAnswerDto answer)
    {
        try
        {
            KanjiReviewResultDto result = await _kanjiService.CheckReviewAsync(GetUserId(), answer);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("learn/{kanjiId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
