import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import { authService } from '@/services/auth.service'
import { api } from '@/services/api'
import type { AuthMeResponse } from '@/types/auth.types'
import type { InstitutionDto, StudyProgramDto, StudyProfileDto } from '@/types/institution.types'

export const useAuthStore = defineStore('auth', () => {
  const initialized = ref(false)
  const user = ref<AuthMeResponse | null>(null)
  const isOnboarded = ref<boolean>(false)
  const role = ref<string | null>(null)
  const institution = ref<InstitutionDto | null>(null)
  const studyProgram = ref<StudyProgramDto | null>(null)
  const studyProfile = ref<StudyProfileDto | null>(null)

  const isLoggedIn = computed(() => user.value?.isAuthenticated === true)
  const email = computed(() => user.value?.email ?? null)
  const name = computed(() => user.value?.name ?? null)
  const sub = computed(() => user.value?.sub ?? null)

  async function init(force = false) {
    if (initialized.value && !force) {
      return
    }

    initialized.value = true

    try {
      const response = await api.get<AuthMeResponse>('/auth/me')
      const data = response.data
      if (data.isAuthenticated) {
        user.value = data
        isOnboarded.value = data.isOnboarded
        role.value = data.role ?? null
        institution.value = data.institution
        studyProgram.value = data.studyProgram
        studyProfile.value = data.studyProfile
        return
      }
    } catch {
      // keep defaults below
    }

    user.value = null
    isOnboarded.value = false
    role.value = null
    institution.value = null
    studyProgram.value = null
    studyProfile.value = null
  }

  function login() {
    authService.login()
  }

  function logout() {
    initialized.value = false
    user.value = null
    isOnboarded.value = false
    role.value = null
    institution.value = null
    studyProgram.value = null
    studyProfile.value = null
    authService.logout()
  }

  return {
    user,
    isOnboarded,
    role,
    institution,
    studyProgram,
    studyProfile,
    email,
    name,
    sub,
    isLoggedIn,
    init,
    login,
    logout
  }
})
