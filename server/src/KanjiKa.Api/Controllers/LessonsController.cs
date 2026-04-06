using System.Security.Claims;
using KanjiKa.Application.DTOs.Learning;
using KanjiKa.Application.Interfaces;
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

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetLessonsCount()
    {
        LessonsCountDto count = await _lessonService.GetLessonsCountAsync(GetUserId());
        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetNewLessons([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 5)
    {
        IEnumerable<LessonDto> lessons = await _lessonService.GetLessonsAsync(GetUserId(), pageIndex, pageSize);
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
        LessonReviewsCountDto count = await _lessonService.GetLessonReviewsCountAsync(GetUserId());
        return Ok(count);
    }

    [HttpGet("reviews")]
    public async Task<IActionResult> GetLessonReviews()
    {
        IEnumerable<LessonReviewDto> reviewItems = await _lessonService.GetLessonReviewsAsync(GetUserId());
        return Ok(reviewItems);
    }

    [HttpPost("reviews/check")]
    public async Task<IActionResult> CheckLessonReviewAnswer([FromBody] LessonReviewAnswerDto answer)
    {
        LessonReviewAnswerResultDto result = await _lessonService.CheckLessonReviewAnswerAsync(GetUserId(), answer);
        return Ok(result);
    }

    [HttpGet("writing-reviews/count")]
    public async Task<IActionResult> GetWritingReviewsCount()
    {
        LessonReviewsCountDto count = await _lessonService.GetWritingReviewsCountAsync(GetUserId());
        return Ok(count);
    }

    [HttpGet("writing-reviews")]
    public async Task<IActionResult> GetWritingReviews()
    {
        IEnumerable<WritingReviewDto> reviewItems = await _lessonService.GetWritingReviewsAsync(GetUserId());
        return Ok(reviewItems);
    }

    [HttpPost("writing-reviews/check")]
    public async Task<IActionResult> CheckWritingReviewAnswer([FromBody] WritingReviewAnswerDto answer)
    {
        LessonReviewAnswerResultDto result = await _lessonService.CheckWritingReviewAnswerAsync(GetUserId(), answer);
        return Ok(result);
    }
}
