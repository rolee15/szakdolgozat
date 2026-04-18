using System.Security.Claims;
using KanjiKa.Application.DTOs.Grammar;
using KanjiKa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/grammar")]
[Produces("application/json")]
public class GrammarController : ControllerBase
{
    private readonly IGrammarService _grammarService;

    public GrammarController(IGrammarService grammarService)
    {
        _grammarService = grammarService;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<GrammarPointDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGrammarPoints()
    {
        List<GrammarPointDto> result = await _grammarService.GetGrammarPointsAsync(GetUserId());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GrammarPointDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrammarPointDetail(int id)
    {
        GrammarPointDetailDto? result = await _grammarService.GetGrammarPointDetailAsync(id, GetUserId());
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("{id:int}/exercises/check")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(GrammarExerciseResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckExercise(int id, [FromBody] GrammarExerciseAnswerDto answer)
    {
        try
        {
            GrammarExerciseResultDto result = await _grammarService.CheckExerciseAsync(GetUserId(), id, answer);
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
