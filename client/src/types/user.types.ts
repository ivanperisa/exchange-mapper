import type { InstitutionDto, StudyProgramDto, StudyProfileDto } from '@/types/institution.types'

export interface UserInstitutionDto {
  userInstitutionId: string
  hasActiveExchanges: boolean
  institution: InstitutionDto
  studyProgram?: StudyProgramDto
  studyProfile?: StudyProfileDto
}

export interface NewInstitutionRequestDto {
  name: string
  nameEn?: string | null
  country: string
  city?: string | null
  erasmusCode?: string | null
  iscedCode?: string | null
  programName?: string | null
  programNameEn?: string | null
  profileName?: string | null
  profileNameEn?: string | null
}

export interface NewStudyProfileRequestDto {
  studyProgramId: string
  profileName: string
  profileNameEn?: string | null
}

export interface OnboardingRequestDto {
  role: 'Student' | 'Coordinator'
  institutions: InstitutionEntryDto[]
}

export interface InstitutionEntryDto {
  existingStudyProfileId?: string | null
  existingInstitutionId?: string | null
  newStudyProfile?: NewStudyProfileRequestDto | null
  newInstitution?: NewInstitutionRequestDto | null
}
