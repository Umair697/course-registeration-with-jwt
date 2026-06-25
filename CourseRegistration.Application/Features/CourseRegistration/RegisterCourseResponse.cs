namespace CourseRegistration.Application.Features.CourseRegistration;

public class RegisterCourseResponse
{
    public Guid RegistrationId { get; set; }

    public string Message { get; set; } = string.Empty;
}