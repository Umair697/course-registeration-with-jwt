using CourseRegistration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseRegistration.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Tenant> Tenants { get; }

    DbSet<Learner> Learners { get; }

    DbSet<Course> Courses { get; }

    DbSet<CourseRegistration.Domain.Entities.CourseRegistration> CourseRegistrations { get; }

    DbSet<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}