namespace CourseRegistration.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Title { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public int RegisteredCount { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public byte[] RowVersion { get; set; } = [];

    public Tenant Tenant { get; set; } = null!;

    public ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();
}