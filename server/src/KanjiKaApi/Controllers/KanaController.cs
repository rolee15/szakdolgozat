using KanjiKa.Core.Dtos;
using KanjiKa.Core.Entities;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKaApi.Controllers;

[ApiController]
[Route("api/characters/{type}")]
public class KanaCharactersController : ControllerBase
{
    private readonly IKanaService _kanaService;

    public KanaCharactersController(IKanaService kanaService)
    {
        _kanaService = kanaService;
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<KanaCharacterDto>>> GetCharacters(string type, string userId)
    {
        var kanaType = Enum.Parse<KanaType>(type, true);
        var characters = await _kanaService.GetKanaCharacters(kanaType, userId);
        return Ok(characters);
    }

    [HttpGet("{character}")]
    public async Task<ActionResult<KanaCharacterDetailDto>> GetCharacterDetail(string type, string character, string userId)
    {
        var kanaType = Enum.Parse<KanaType>(type, true);
        var charDetail = await _kanaService.GetCharacterDetail(character, kanaType, userId);
        return Ok(charDetail);
    }

    [HttpGet("{character}/examples")]
    public async Task<ActionResult<IEnumerable<string>>> GetExamples(string type, string character)
    {
        var kanaType = Enum.Parse<KanaType>(type, true);
        var examples = await _kanaService.GetExamples(character, kanaType);
        return Ok(examples);
    }
}
