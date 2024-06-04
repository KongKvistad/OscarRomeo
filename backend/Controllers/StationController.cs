using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowLocalhost3000")]
public class StationController : ControllerBase
{
    private readonly StationService _stationService;

    public StationController(StationService stationService)
    {
        _stationService = stationService;
    }

    [HttpGet("stationinfo")]
    public async Task<IActionResult> GetStationInformation()
    {
        var stationData = await _stationService.GetMergedStationDataAsync();
        return Ok(stationData);
    }
}

