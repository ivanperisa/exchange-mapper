using ExchangeMapper.Application.DTOs;
using ExchangeMapper.Application.Interfaces;
using ExchangeMapper.Domain.Entities;
using ExchangeMapper.Domain.Enums;

namespace ExchangeMapper.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IInstitutionRepository institutionRepository,
    IStudyProgramRepository studyProgramRepository,
    IStudyProfileRepository studyProfileRepository) : IUserService
{
    public async Task<User> SyncUserAsync(string externalId, string email, string name)
    {
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

    public Task<User?> GetByExternalIdAsync(string externalId)
    {
        return userRepository.GetByExternalIdAsync(externalId);
    }

    public Task<User?> GetByExternalIdWithDetailsAsync(string externalId)
    {
        return userRepository.GetByExternalIdWithDetailsAsync(externalId);
    }

    public async Task CompleteOnboardingAsync(Guid userId, OnboardingRequestDto request)
    {
        var user = await userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        Guid? institutionId;
        Guid? studyProfileId;

        if (request.NewInstitution is not null)
        {
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
            var institution = await institutionRepository.GetByIdAsync(request.ExistingInstitutionId.Value)
                ?? throw new InvalidOperationException("Institution not found.");

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
            var profile = await studyProfileRepository.GetByIdAsync(request.ExistingStudyProfileId.Value)
                ?? throw new KeyNotFoundException("Study profile not found.");
            var program = await studyProgramRepository.GetByIdAsync(profile.StudyProgramId)
                ?? throw new KeyNotFoundException("Study program not found.");

            studyProfileId = profile.Id;
            institutionId = program.InstitutionId;
        }
        else if (request.Role == UserRole.Coordinator && request.ExistingInstitutionId.HasValue)
        {
            institutionId = request.ExistingInstitutionId.Value;
            studyProfileId = null;
        }
        else
        {
            throw new InvalidOperationException("Onboarding request does not contain valid institution/study profile data.");
        }

        user.Role = request.Role;
        user.InstitutionId = institutionId;
        user.StudyProfileId = studyProfileId;
        user.IsOnboarded = true;

        await userRepository.UpdateAsync(user);
    }

    public async Task MakeCoordinatorAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        user.Role = UserRole.Coordinator;
        await userRepository.UpdateAsync(user);
    }
}
