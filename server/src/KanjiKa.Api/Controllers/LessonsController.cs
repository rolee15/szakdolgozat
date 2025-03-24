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

    [HttpGet("todayCount")]
    public async Task<IActionResult> GetTodayLessonCount([FromQuery] int userId)
    {
        var count = await _lessonService.GetTodayLessonCountAsync(userId);
        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetNewLessons([FromQuery] int userId, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 5)
    {
        var lessons = await _lessonService.GetNewLessonsAsync(userId, pageIndex, pageSize);
        return Ok(lessons);
    }

    [HttpPost("learnLesson/{characterId:int}")]
    public async Task<IActionResult> LearnLesson(int characterId, [FromQuery] int userId)
    {
        await _lessonService.LearnLessonAsync(userId, characterId);
        return Ok();
    }

    [HttpGet("reviews")]
    public async Task<IActionResult> GetLessonReviews([FromQuery] int userId)
    {
        var reviewItems = await _lessonService.GetLessonReviews(userId);
        return Ok(reviewItems);
    }

    [HttpPost("reviews/items/{character}")]
    public async Task<IActionResult> CheckReviewItemAnswer(string character, [FromQuery] int userId, [FromBody] LessonReviewAnswerDto answer)
    {
        var result = await _lessonService.CheckReviewItemAnswerAsync(userId, character, answer);
        return Ok(result);
    }
}