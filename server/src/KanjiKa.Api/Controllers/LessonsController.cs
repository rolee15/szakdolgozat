using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Route("api/lessons")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetLessonsCount([FromQuery] int userId)
    {
        var count = await _lessonService.GetLessonsCountAsync(userId);
        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetNewLessons([FromQuery] int userId, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 5)
    {
        var lessons = await _lessonService.GetLessonsAsync(userId, pageIndex, pageSize);
        return Ok(lessons);
    }

    [HttpPost("learn/{characterId:int}")]
    public async Task<IActionResult> LearnLesson(int characterId, [FromQuery] int userId)
    {
        await _lessonService.LearnLessonAsync(userId, characterId);
        return Ok();
    }

    [HttpGet("reviews/count")]
    public async Task<IActionResult> GetReviewCount([FromQuery] int userId)
    {
        var count = await _lessonService.GetLessonReviewsCountAsync(userId);
        return Ok(count);
    }

    [HttpGet("reviews")]
    public async Task<IActionResult> GetLessonReviews([FromQuery] int userId)
    {
        var reviewItems = await _lessonService.GetLessonReviewsAsync(userId);
        return Ok(reviewItems);
    }

    [HttpPost("reviews/check")]
    public async Task<IActionResult> CheckLessonReviewAnswer([FromQuery] int userId, [FromBody] LessonReviewAnswerDto answer)
    {
        var result = await _lessonService.CheckLessonReviewAnswerAsync(userId, answer);
        return Ok(result);
    }
}