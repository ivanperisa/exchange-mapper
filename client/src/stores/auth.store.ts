import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import { authService } from '@/services/auth.service'
import { userService } from '@/services/user.service'
import { api } from '@/services/api'
import type { AuthMeResponse } from '@/types/auth.types'
import type { InstitutionEntryDto, UserInstitutionDto } from '@/types/user.types'

export const useAuthStore = defineStore('auth', () => {
  const initialized = ref(false)
  const user = ref<AuthMeResponse | null>(null)
  const isOnboarded = ref<boolean>(false)
  const role = ref<string | null>(null)
  const institutions = ref<UserInstitutionDto[]>([])

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
        institutions.value = data.institutions ?? []
        return
      }
    } catch {
      // keep defaults below
    }

    user.value = null
    isOnboarded.value = false
    role.value = null
    institutions.value = []
  }

  function login() {
    authService.login()
  }

  function logout() {
    initialized.value = false
    user.value = null
    isOnboarded.value = false
    role.value = null
    institutions.value = []
    authService.logout()
  }

  async function addInstitution(request: InstitutionEntryDto): Promise<void> {
    await userService.addInstitution(request)
    await init(true)
  }

  async function updateInstitution(userInstitutionId: string, request: InstitutionEntryDto): Promise<void> {
    await userService.updateInstitution(userInstitutionId, request)
    await init(true)
  }

  async function removeInstitution(userInstitutionId: string): Promise<void> {
    await userService.removeInstitution(userInstitutionId)
    institutions.value = institutions.value.filter((i) => i.userInstitutionId !== userInstitutionId)
    if (user.value) {
      user.value = { ...user.value, institutions: institutions.value }
    }
  }

  return {
    user,
    isOnboarded,
    role,
    institutions,
    email,
    name,
    sub,
    isLoggedIn,
    init,
    login,
    logout,
    addInstitution,
    updateInstitution,
    removeInstitution
  }
})
