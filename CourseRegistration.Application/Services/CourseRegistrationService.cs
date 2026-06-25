using CourseRegistration.Application.Abstractions;
using CourseRegistration.Application.Common.Exceptions;
using CourseRegistration.Application.Features.CourseRegistration;
using CourseRegistration.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CourseRegistration.Application.Services;

public class CourseRegistrationService : ICourseRegistrationService
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CourseRegistrationService> _logger;

    public CourseRegistrationService(
        IApplicationDbContext context,
        ILogger<CourseRegistrationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<RegisterCourseResponse> RegisterAsync(
        RegisterCourseRequest request,
        Guid tenantId,
        string performedBy,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Course registration started. TenantId: {TenantId}, LearnerId: {LearnerId}, CourseId: {CourseId}",
            tenantId,
            request.LearnerId,
            request.CourseId);

        var learnerExists = await _context.Learners
            .AnyAsync(x =>
                x.Id == request.LearnerId &&
                x.TenantId == tenantId &&
                x.IsActive,
                cancellationToken);

        if (!learnerExists)
        {
            throw new NotFoundException("Learner was not found for this tenant.");
        }

        var course = await _context.Courses
            .FirstOrDefaultAsync(x =>
                x.Id == request.CourseId &&
                x.TenantId == tenantId &&
                x.IsActive,
                cancellationToken);

        if (course is null)
        {
            throw new NotFoundException("Course was not found for this tenant.");
        }

        var alreadyRegistered = await _context.CourseRegistrations
            .AnyAsync(x =>
                x.TenantId == tenantId &&
                x.LearnerId == request.LearnerId &&
                x.CourseId == request.CourseId,
                cancellationToken);

        if (alreadyRegistered)
        {
            throw new ConflictException("Learner is already registered for this course.");
        }

        if (course.RegisteredCount >= course.Capacity)
        {
            throw new ConflictException("Course capacity has been exceeded.");
        }

        var registration = new Domain.Entities.CourseRegistration
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            LearnerId = request.LearnerId,
            CourseId = request.CourseId,
            RegisteredAtUtc = DateTime.UtcNow,
            RegisteredBy = performedBy
        };

        course.RegisteredCount++;

        _context.CourseRegistrations.Add(registration);

        _context.AuditLogs.Add(new AuditLog
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Action = "CourseRegistered",
            EntityName = "CourseRegistration",
            EntityId = registration.Id.ToString(),
            PerformedBy = performedBy,
            CreatedAtUtc = DateTime.UtcNow,
            Details = $"Learner {request.LearnerId} registered for course {request.CourseId}"
        });

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Course registration completed. RegistrationId: {RegistrationId}",
            registration.Id);

        return new RegisterCourseResponse
        {
            RegistrationId = registration.Id,
            Message = "Course registered successfully."
        };
    }
}