using CourseRegistration.Application.Features.CourseRegistration;

namespace CourseRegistration.Application.Abstractions;

public interface ICourseRegistrationService
{
    Task<RegisterCourseResponse> RegisterAsync(
        RegisterCourseRequest request,
        Guid tenantId,
        string performedBy,
        CancellationToken cancellationToken);
}