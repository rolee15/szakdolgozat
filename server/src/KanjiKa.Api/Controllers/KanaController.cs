using System.Security.Claims;
using KanjiKa.Application.DTOs.Kana;
using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/characters/{type}")]
[Produces("application/json")]
public class KanaCharactersController : ControllerBase
{
    private readonly IKanaService _kanaService;

    public KanaCharactersController(IKanaService kanaService)
    {
        _kanaService = kanaService;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(IEnumerable<KanaCharacterDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<KanaCharacterDto>>> GetCharacters(string type)
    {
        var kanaType = Enum.Parse<KanaType>(type, true);
        IEnumerable<KanaCharacterDto> characters = await _kanaService.GetKanaCharacters(kanaType, GetUserId());
        return Ok(characters);
    }

    [HttpGet("{character}")]
    [ProducesResponseType(typeof(KanaCharacterDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<KanaCharacterDetailDto>> GetCharacterDetail(string type, string character)
    {
        var kanaType = Enum.Parse<KanaType>(type, true);
        KanaCharacterDetailDto charDetail = await _kanaService.GetCharacterDetail(character, kanaType, GetUserId());
        return Ok(charDetail);
    }

    [HttpGet("{character}/examples")]
    [ProducesResponseType(typeof(IEnumerable<ExampleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetExamples(string type, string character)
    {
        var kanaType = Enum.Parse<KanaType>(type, true);
        IEnumerable<ExampleDto> examples = await _kanaService.GetExamples(character, kanaType);
        return Ok(examples);
    }
}
