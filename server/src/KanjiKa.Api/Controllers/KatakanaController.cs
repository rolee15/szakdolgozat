using System.Security.Claims;
using KanjiKa.Application.DTOs.Kana;
using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/katakana")]
[Produces("application/json")]
public class KatakanaController : ControllerBase
{
    private readonly IKanaService _kanaService;

    public KatakanaController(IKanaService kanaService)
    {
        _kanaService = kanaService;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(IEnumerable<KanaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<KanaDto>>> GetCharacters()
    {
        IEnumerable<KanaDto> characters = await _kanaService.GetKanaCharacters(KanaType.Katakana, GetUserId());
        return Ok(characters);
    }

    [HttpGet("{character}")]
    [ProducesResponseType(typeof(KanaDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<KanaDetailDto>> GetCharacterDetail(string character)
    {
        KanaDetailDto charDetail = await _kanaService.GetCharacterDetail(character, KanaType.Katakana, GetUserId());
        return Ok(charDetail);
    }

    [HttpGet("{character}/examples")]
    [ProducesResponseType(typeof(IEnumerable<KanaExampleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetExamples(string character)
    {
        IEnumerable<KanaExampleDto> examples = await _kanaService.GetExamples(character, KanaType.Katakana);
        return Ok(examples);
    }
}
