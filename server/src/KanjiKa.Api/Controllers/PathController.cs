using System.Security.Claims;
using KanjiKa.Core.DTOs.Path;
using KanjiKa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanjiKa.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/path")]
public class PathController : ControllerBase
{
    private readonly IPathService _pathService;

    public PathController(IPathService pathService)
    {
        _pathService = pathService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetPath()
    {
        var result = await _pathService.GetPathAsync(GetUserId());
        return Ok(result);
    }

    [HttpGet("{unitId:int}")]
    public async Task<IActionResult> GetUnitDetail(int unitId)
    {
        var result = await _pathService.GetUnitDetailAsync(unitId, GetUserId());
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{unitId:int}/test")]
    public async Task<IActionResult> GetUnitTest(int unitId)
    {
        var result = await _pathService.GetUnitTestAsync(unitId, GetUserId());
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("{unitId:int}/test")]
    public async Task<IActionResult> SubmitTest(int unitId, [FromBody] UnitSubmitDto submitDto)
    {
        try
        {
            var result = await _pathService.SubmitTestAsync(GetUserId(), unitId, submitDto);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
