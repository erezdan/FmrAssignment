using Microsoft.AspNetCore.Mvc;
using FmrServer.Services;
using FmrModels;
using FmrModels.Models;

namespace FmrServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SharesController : ControllerBase
{
    private readonly ShareService _shareService;

    public SharesController(ShareService shareService)
    {
        _shareService = shareService;
    }

    // GET api/shares
    [HttpGet]
    public ActionResult<List<Share>> GetAllShares()
    {
        var allShares = _shareService.GetAllShares();
        return Ok(allShares);
    }

    // GET api/shares/updates
    [HttpGet("updates")]
    public IActionResult GetUpdatesSince([FromQuery] DateTime since)
    {
        var updated = _shareService.GetUpdatedSharesSince(since);
        return Ok(updated);
    }
}
