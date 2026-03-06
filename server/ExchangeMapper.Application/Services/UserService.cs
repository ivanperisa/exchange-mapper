using ErrorOr;
using ExchangeMapper.Application.DTOs.Requests;
using ExchangeMapper.Application.Interfaces.Repositories;
using ExchangeMapper.Application.Interfaces.Services;
using ExchangeMapper.Domain.Entities;
using ExchangeMapper.Domain.Enums;

namespace ExchangeMapper.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IInstitutionRepository institutionRepository,
    IStudyProgramRepository studyProgramRepository,
    IStudyProfileRepository studyProfileRepository) : IUserService
{
    public async Task<ErrorOr<User>> SyncUserAsync(string externalId, string email, string name)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            return Error.Validation("INVALID_EXTERNAL_ID", "External id is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Error.Validation("INVALID_EMAIL", "Email is required.");
        }

        var existingUser = await userRepository.GetByExternalIdAsync(externalId);
        if (existingUser is not null)
        {
            return existingUser;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            Email = email,
            Name = name,
            Role = UserRole.Student,
            IsOnboarded = false,
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.AddAsync(user);
        return user;
    }

    public async Task<ErrorOr<User>> GetByExternalIdAsync(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            return Error.Validation("INVALID_EXTERNAL_ID", "External id is required.");
        }

        var user = await userRepository.GetByExternalIdAsync(externalId);
        return user is null
            ? Error.NotFound("USER_NOT_FOUND", "User not found.")
            : user;
    }

    public async Task<ErrorOr<User>> GetByExternalIdWithDetailsAsync(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            return Error.Validation("INVALID_EXTERNAL_ID", "External id is required.");
        }

        var user = await userRepository.GetByExternalIdWithDetailsAsync(externalId);
        return user is null
            ? Error.NotFound("USER_NOT_FOUND", "User not found.")
            : user;
    }

    public async Task<ErrorOr<Success>> CompleteOnboardingAsync(Guid userId, CompleteOnboardingRequestDto request)
    {
        if (!Enum.IsDefined(request.Role))
        {
            return Error.Validation("INVALID_ROLE", "Provided role is not valid.");
        }

        if (request.Role == UserRole.Student
            && request.ExistingStudyProfileId is null
            && request.NewStudyProfile is null
            && request.NewInstitution is null)
        {
            return Error.Validation("MISSING_STUDY_PROFILE", "Students must provide study profile information.");
        }

        if (request.Role == UserRole.Coordinator
            && request.ExistingInstitutionId is null
            && request.NewInstitution is null)
        {
            return Error.Validation("MISSING_INSTITUTION", "Coordinators must provide an institution.");
        }

        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            return Error.NotFound("USER_NOT_FOUND", "User not found.");
        }

        Guid? institutionId;
        Guid? studyProfileId;

        if (request.NewInstitution is not null)
        {
            if (string.IsNullOrWhiteSpace(request.NewInstitution.Name)
                || string.IsNullOrWhiteSpace(request.NewInstitution.Country)
                || string.IsNullOrWhiteSpace(request.NewInstitution.IscedCode)
                || string.IsNullOrWhiteSpace(request.NewInstitution.ProgramName)
                || string.IsNullOrWhiteSpace(request.NewInstitution.ProfileName))
            {
                return Error.Validation(
                    "INVALID_NEW_INSTITUTION",
                    "New institution requires name, country, ISCED code, program name, and profile name.");
            }

            var institution = new Institution
            {
                Id = Guid.NewGuid(),
                Name = request.NewInstitution.Name,
                Country = request.NewInstitution.Country,
                City = request.NewInstitution.City,
                ErasmusCode = request.NewInstitution.ErasmusCode,
                IsHome = true
            };
            await institutionRepository.AddAsync(institution);

            var studyProgram = new StudyProgram
            {
                Id = Guid.NewGuid(),
                InstitutionId = institution.Id,
                Name = request.NewInstitution.ProgramName,
                IscedCode = request.NewInstitution.IscedCode
            };
            await studyProgramRepository.AddAsync(studyProgram);

            var studyProfile = new StudyProfile
            {
                Id = Guid.NewGuid(),
                StudyProgramId = studyProgram.Id,
                Name = request.NewInstitution.ProfileName
            };
            await studyProfileRepository.AddAsync(studyProfile);

            institutionId = institution.Id;
            studyProfileId = studyProfile.Id;
        }
        else if (request.ExistingInstitutionId.HasValue && request.NewStudyProfile is not null && request.Role == UserRole.Student)
        {
            if (request.NewStudyProfile.StudyProgramId == Guid.Empty || string.IsNullOrWhiteSpace(request.NewStudyProfile.ProfileName))
            {
                return Error.Validation("INVALID_NEW_STUDY_PROFILE", "Study program id and profile name are required.");
            }

            var institution = await institutionRepository.GetByIdAsync(request.ExistingInstitutionId.Value);
            if (institution is null)
            {
                return Error.NotFound("INSTITUTION_NOT_FOUND", "Institution not found.");
            }

            var studyProgram = await studyProgramRepository.GetByIdAsync(request.NewStudyProfile.StudyProgramId);
            if (studyProgram is null || studyProgram.InstitutionId != institution.Id)
            {
                return Error.Validation("INVALID_STUDY_PROGRAM", "Study program does not belong to the selected institution.");
            }

            var studyProfile = new StudyProfile
            {
                Id = Guid.NewGuid(),
                StudyProgramId = request.NewStudyProfile.StudyProgramId,
                Name = request.NewStudyProfile.ProfileName
            };
            await studyProfileRepository.AddAsync(studyProfile);

            institutionId = institution.Id;
            studyProfileId = studyProfile.Id;
        }
        else if (request.ExistingStudyProfileId.HasValue)
        {
            var profile = await studyProfileRepository.GetByIdAsync(request.ExistingStudyProfileId.Value);
            if (profile is null)
            {
                return Error.NotFound("STUDY_PROFILE_NOT_FOUND", "Study profile not found.");
            }

            var program = await studyProgramRepository.GetByIdAsync(profile.StudyProgramId);
            if (program is null)
            {
                return Error.NotFound("STUDY_PROGRAM_NOT_FOUND", "Study program not found.");
            }

            studyProfileId = profile.Id;
            institutionId = program.InstitutionId;
        }
        else if (request.Role == UserRole.Coordinator && request.ExistingInstitutionId.HasValue)
        {
            var institution = await institutionRepository.GetByIdAsync(request.ExistingInstitutionId.Value);
            if (institution is null)
            {
                return Error.NotFound("INSTITUTION_NOT_FOUND", "Institution not found.");
            }

            institutionId = institution.Id;
            studyProfileId = null;
        }
        else
        {
            return Error.Validation(
                "INVALID_ONBOARDING_DATA",
                "Onboarding request does not contain valid institution/study profile data.");
        }

        user.Role = request.Role;
        user.InstitutionId = institutionId;
        user.StudyProfileId = studyProfileId;
        user.IsOnboarded = true;

        await userRepository.UpdateAsync(user);
        return Result.Success;
    }

    public async Task<ErrorOr<Success>> MakeCoordinatorAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            return Error.NotFound("USER_NOT_FOUND", "User not found.");
        }

        user.Role = UserRole.Coordinator;
        user.IsOnboarded = true;
        await userRepository.UpdateAsync(user);
        return Result.Success;
    }
}
