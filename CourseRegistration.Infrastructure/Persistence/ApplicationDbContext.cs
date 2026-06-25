using CourseRegistration.Application.Abstractions;
using CourseRegistration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseRegistration.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();

    public DbSet<Learner> Learners => Set<Learner>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Domain.Entities.CourseRegistration> CourseRegistrations => Set<Domain.Entities.CourseRegistration>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}