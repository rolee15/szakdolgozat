using System.Security.Claims;
using KanjiKa.Application.DTOs.Grammar;
using KanjiKa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/grammar")]
public class GrammarController : ControllerBase
{
    private readonly IGrammarService _grammarService;

    public GrammarController(IGrammarService grammarService)
    {
        _grammarService = grammarService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetGrammarPoints()
    {
        var result = await _grammarService.GetGrammarPointsAsync(GetUserId());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGrammarPointDetail(int id)
    {
        var result = await _grammarService.GetGrammarPointDetailAsync(id, GetUserId());
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("{id:int}/exercises/check")]
    public async Task<IActionResult> CheckExercise(int id, [FromBody] GrammarExerciseAnswerDto answer)
    {
        try
        {
            var result = await _grammarService.CheckExerciseAsync(GetUserId(), id, answer);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
    }
}
