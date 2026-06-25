using FluentValidation;

namespace CourseRegistration.Application.Features.CourseRegistration;

public class RegisterCourseRequestValidator : AbstractValidator<RegisterCourseRequest>
{
    public RegisterCourseRequestValidator()
    {
        RuleFor(x => x.LearnerId)
            .NotEmpty()
            .WithMessage("LearnerId is required.");

        RuleFor(x => x.CourseId)
            .NotEmpty()
            .WithMessage("CourseId is required.");
    }
}