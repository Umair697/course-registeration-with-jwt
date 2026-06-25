namespace CourseRegistration.Application.Features.CourseRegistration;

public class RegisterCourseRequest
{
    public Guid LearnerId { get; set; }

    public Guid CourseId { get; set; }
}