namespace CourseRegistration.Domain.Entities;

public class Learner
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Tenant Tenant { get; set; } = null!;

    public ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();
}