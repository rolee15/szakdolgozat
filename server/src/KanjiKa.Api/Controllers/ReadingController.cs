using System.Security.Claims;
using KanjiKa.Core.DTOs.Reading;
using KanjiKa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/reading")]
public class ReadingController : ControllerBase
{
    private readonly IReadingService _readingService;

    public ReadingController(IReadingService readingService)
    {
        _readingService = readingService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetPassages()
    {
        var result = await _readingService.GetPassagesAsync(GetUserId());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPassageDetail(int id)
    {
        var result = await _readingService.GetPassageDetailAsync(id, GetUserId());
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("{id:int}/submit")]
    public async Task<IActionResult> SubmitAnswers(int id, [FromBody] ReadingSubmitDto submitDto)
    {
        if (submitDto.PassageId != id)
            submitDto.PassageId = id;

        try
        {
            var result = await _readingService.SubmitAnswersAsync(GetUserId(), submitDto);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
