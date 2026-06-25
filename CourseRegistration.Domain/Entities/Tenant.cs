namespace CourseRegistration.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Learner> Learners { get; set; } = new List<Learner>();

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}