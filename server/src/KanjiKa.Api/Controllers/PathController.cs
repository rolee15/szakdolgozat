using System.Security.Claims;
using KanjiKa.Application.DTOs.Path;
using KanjiKa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/path")]
[Produces("application/json")]
public class PathController : ControllerBase
{
    private readonly IPathService _pathService;

    public PathController(IPathService pathService)
    {
        _pathService = pathService;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<LearningUnitDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPath()
    {
        List<LearningUnitDto> result = await _pathService.GetPathAsync(GetUserId());
        return Ok(result);
    }

    [HttpGet("{unitId:int}")]
    [ProducesResponseType(typeof(LearningUnitDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnitDetail(int unitId)
    {
        LearningUnitDetailDto? result = await _pathService.GetUnitDetailAsync(unitId, GetUserId());
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpGet("{unitId:int}/test")]
    [ProducesResponseType(typeof(UnitTestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnitTest(int unitId)
    {
        UnitTestDto? result = await _pathService.GetUnitTestAsync(unitId, GetUserId());
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("{unitId:int}/test")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UnitTestResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitTest(int unitId, [FromBody] UnitSubmitDto submitDto)
    {
        try
        {
            UnitTestResultDto result = await _pathService.SubmitTestAsync(GetUserId(), unitId, submitDto);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
