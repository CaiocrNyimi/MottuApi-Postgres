using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MottuApi.Services.Interfaces;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/entrega")]
public class EntregaMlController : ControllerBase
{
    private readonly IEntregaMlService _mlService;

    public EntregaMlController(IEntregaMlService mlService)
    {
        _mlService = mlService;
    }

    [HttpPost("prever-tempo")]
    public ActionResult<EntregaResponseDto> PreverTempoEntrega([FromBody] EntregaRequestDto request)
    {
        var response = _mlService.PreverTempoEntregaDto(request);
        return Ok(response);
    }
}