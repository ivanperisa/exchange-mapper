import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authService } from '@/services/auth.service'
import type { User } from 'oidc-client-ts'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const callbackError = ref<string | null>(null)

  const isLoggedIn = computed(() => !!user.value && !user.value.expired)
  const accessToken = computed(() => user.value?.access_token)

  async function init() {
    user.value = await authService.getUser()
  }

  async function login(returnTo: string = '/') {
    await authService.login(returnTo)
  }

  async function logout() {
    await authService.logout()
    user.value = null
  }

  async function handleCallback() {
    callbackError.value = null
    user.value = await authService.handleCallback()
  }

  return { user, callbackError, isLoggedIn, accessToken, init, login, logout, handleCallback }
})
