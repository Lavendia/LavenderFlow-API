using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LabelsController : ControllerBase
{
    private readonly ILabelService _service;

    public LabelsController(ILabelService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetLabels()
    {
        return Ok(await _service.GetLabelsAsync());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLabel(int id)
    {
        var label = await _service.GetLabelAsync(id);
        return label is null ? NotFound() : Ok(label);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateLabel([FromBody] CreateLabelRequest request)
    {
        var label = await _service.CreateLabelAsync(request);
        return label is null
            ? BadRequest("Failed to create label")
            : CreatedAtAction(nameof(GetLabel), new { id = label.Id }, label);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLabel(int id, [FromBody] UpdateLabelRequest request)
    {
        var label = await _service.UpdateLabelAsync(id, request);
        return label is null ? NotFound() : Ok(label);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLabel(int id)
    {
        return await _service.DeleteLabelAsync(id)
            ? NoContent()
            : NotFound();
    }
}
