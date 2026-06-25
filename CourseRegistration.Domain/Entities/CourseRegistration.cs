namespace CourseRegistration.Domain.Entities;

public class CourseRegistration
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid LearnerId { get; set; }

    public Guid CourseId { get; set; }

    public DateTime RegisteredAtUtc { get; set; } = DateTime.UtcNow;

    public string RegisteredBy { get; set; } = string.Empty;

    public Learner Learner { get; set; } = null!;

    public Course Course { get; set; } = null!;
}