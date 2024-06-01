using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        var stationInfo = await _stationService.GetStationInformationAsync();
        return Ok(stationInfo);
    }
}

