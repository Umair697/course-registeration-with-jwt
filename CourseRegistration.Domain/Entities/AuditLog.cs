namespace CourseRegistration.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public string EntityId { get; set; } = string.Empty;

    public string PerformedBy { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public string? Details { get; set; }
}