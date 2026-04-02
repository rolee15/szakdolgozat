using System.Security.Claims;
using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/lessons")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("count")]
    public async Task<IActionResult> GetLessonsCount()
    {
        var count = await _lessonService.GetLessonsCountAsync(GetUserId());
        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetNewLessons([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 5)
    {
        var lessons = await _lessonService.GetLessonsAsync(GetUserId(), pageIndex, pageSize);
        return Ok(lessons);
    }

    [HttpPost("learn/{characterId:int}")]
    public async Task<IActionResult> LearnLesson(int characterId)
    {
        await _lessonService.LearnLessonAsync(GetUserId(), characterId);
        return Ok();
    }

    [HttpGet("reviews/count")]
    public async Task<IActionResult> GetReviewCount()
    {
        var count = await _lessonService.GetLessonReviewsCountAsync(GetUserId());
        return Ok(count);
    }

    [HttpGet("reviews")]
    public async Task<IActionResult> GetLessonReviews()
    {
        var reviewItems = await _lessonService.GetLessonReviewsAsync(GetUserId());
        return Ok(reviewItems);
    }

    [HttpPost("reviews/check")]
    public async Task<IActionResult> CheckLessonReviewAnswer([FromBody] LessonReviewAnswerDto answer)
    {
        var result = await _lessonService.CheckLessonReviewAnswerAsync(GetUserId(), answer);
        return Ok(result);
    }

    [HttpGet("writing-reviews/count")]
    public async Task<IActionResult> GetWritingReviewsCount()
    {
        var count = await _lessonService.GetWritingReviewsCountAsync(GetUserId());
        return Ok(count);
    }

    [HttpGet("writing-reviews")]
    public async Task<IActionResult> GetWritingReviews()
    {
        var reviewItems = await _lessonService.GetWritingReviewsAsync(GetUserId());
        return Ok(reviewItems);
    }

    [HttpPost("writing-reviews/check")]
    public async Task<IActionResult> CheckWritingReviewAnswer([FromBody] WritingReviewAnswerDto answer)
    {
        var result = await _lessonService.CheckWritingReviewAnswerAsync(GetUserId(), answer);
        return Ok(result);
    }
}
