import { api } from '@/services/api'
import type { InstitutionEntryDto, OnboardingRequestDto } from '@/types/user.types'

export const userService = {
  completeOnboarding(request: OnboardingRequestDto) {
    return api.post('/user/onboarding', request)
  },

  addInstitution(request: InstitutionEntryDto) {
    return api.post('/user/institution', request)
  },

  updateInstitution(userInstitutionId: string, request: InstitutionEntryDto) {
    return api.put(`/user/institution/${userInstitutionId}`, request)
  },

  removeInstitution(userInstitutionId: string) {
    return api.delete(`/user/institution/${userInstitutionId}`)
  }
}
