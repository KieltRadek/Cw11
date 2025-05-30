using Cw11.DTOs;
using Cw11.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cw11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _svc;
    public PrescriptionsController(IPrescriptionService svc) => _svc = svc;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NewPrescriptionDto dto)
    {
        try
        {
            var id = await _svc.CreatePrescriptionAsync(dto);
            return CreatedAtAction(null, new { id }, null);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}