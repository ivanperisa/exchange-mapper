import type { InstitutionDto, StudyProgramDto, StudyProfileDto } from '@/types/institution.types'

export interface AuthMeResponse {
  isAuthenticated: boolean
  sub: string
  email: string
  name: string
  role: string
  isOnboarded: boolean
  institution: InstitutionDto | null
  studyProgram: StudyProgramDto | null
  studyProfile: StudyProfileDto | null
}
