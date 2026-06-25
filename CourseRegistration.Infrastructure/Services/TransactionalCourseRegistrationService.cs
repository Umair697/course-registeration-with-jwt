using CourseRegistration.Application.Abstractions;
using CourseRegistration.Application.Common.Exceptions;
using CourseRegistration.Application.Features.CourseRegistration;
using CourseRegistration.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CourseRegistration.Infrastructure.Services;

public class TransactionalCourseRegistrationService : ICourseRegistrationService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CourseRegistration.Application.Services.CourseRegistrationService _innerService;
    private readonly ILogger<TransactionalCourseRegistrationService> _logger;

    public TransactionalCourseRegistrationService(
        ApplicationDbContext dbContext,
        CourseRegistration.Application.Services.CourseRegistrationService innerService,
        ILogger<TransactionalCourseRegistrationService> logger)
    {
        _dbContext = dbContext;
        _innerService = innerService;
        _logger = logger;
    }

    public async Task<RegisterCourseResponse> RegisterAsync(
        RegisterCourseRequest request,
        Guid tenantId,
        string performedBy,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await _innerService.RegisterAsync(
                request,
                tenantId,
                performedBy,
                cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogWarning(
                ex,
                "Concurrency conflict while registering learner {LearnerId} for course {CourseId}",
                request.LearnerId,
                request.CourseId);

            throw new ConflictException("Registration failed due to concurrent request. Please try again.");
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogWarning(
                ex,
                "Database update conflict while registering learner {LearnerId} for course {CourseId}",
                request.LearnerId,
                request.CourseId);

            throw new ConflictException("Learner is already registered for this course or registration conflict occurred.");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}