using System.Security.Claims;
using CourseRegistration.Api.Filters;
using CourseRegistration.Application.Abstractions;
using CourseRegistration.Application.Features.CourseRegistration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseRegistration.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/course")]
public class CourseController : ControllerBase
{
    private readonly ICourseRegistrationService _courseRegistrationService;

    public CourseController(ICourseRegistrationService courseRegistrationService)
    {
        _courseRegistrationService = courseRegistrationService;
    }

    [HttpPost("register")]
    [ServiceFilter(typeof(ValidationFilter<RegisterCourseRequest>))]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCourseRequest request,
        CancellationToken cancellationToken)
    {
        var tenantIdClaim = User.FindFirst("tenantId")?.Value;

        if (string.IsNullOrWhiteSpace(tenantIdClaim))
        {
            return BadRequest(new
            {
                message = "TenantId claim is missing from token."
            });
        }

        var tenantId = Guid.Parse(tenantIdClaim);

        var performedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("sub")?.Value
                          ?? "system";

        var response = await _courseRegistrationService.RegisterAsync(
            request,
            tenantId,
            performedBy,
            cancellationToken);

        return Ok(response);
    }
}