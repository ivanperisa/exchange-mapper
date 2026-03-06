using ExchangeMapper.Application.DTOs;
using ExchangeMapper.Domain.Enums;
using FluentValidation;

namespace ExchangeMapper.Application.Validators;

public class OnboardingRequestValidator : AbstractValidator<OnboardingRequestDto>
{
    public OnboardingRequestValidator()
    {
        RuleFor(x => x.Role).IsInEnum();

        RuleFor(x => x)
            .Must(x => x.ExistingStudyProfileId.HasValue
                    || x.ExistingInstitutionId.HasValue
                    || x.NewInstitution is not null)
            .WithMessage("Must provide either an existing profile, institution, or new institution data.");

        When(x => x.Role == UserRole.Student, () =>
        {
            RuleFor(x => x)
                .Must(x => x.ExistingStudyProfileId.HasValue
                        || x.ExistingInstitutionId.HasValue
                        || x.NewInstitution is not null)
                .WithMessage("Students must provide study profile information.");
        });

        When(x => x.NewInstitution is not null, () =>
        {
            RuleFor(x => x.NewInstitution!.Name).NotEmpty();
            RuleFor(x => x.NewInstitution!.Country).NotEmpty();
            RuleFor(x => x.NewInstitution!.IscedCode).NotEmpty();
            RuleFor(x => x.NewInstitution!.ProgramName).NotEmpty();
            RuleFor(x => x.NewInstitution!.ProfileName).NotEmpty();
        });
    }
}
