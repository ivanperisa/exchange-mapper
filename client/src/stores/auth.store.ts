import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import axios from 'axios'
import { authService } from '@/services/auth.service'

type AuthMeResponse = {
  isAuthenticated: boolean
  email?: string | null
  name?: string | null
  sub?: string | null
}

export const useAuthStore = defineStore('auth', () => {
  const isAuthenticated = ref(false)
  const email = ref<string | null>(null)
  const name = ref<string | null>(null)
  const sub = ref<string | null>(null)

  const isLoggedIn = computed(() => isAuthenticated.value)

  async function init() {
    try {
      const response = await axios.get<AuthMeResponse>('http://localhost:5000/auth/me', { withCredentials: true })
      const data = response.data
      isAuthenticated.value = data.isAuthenticated === true
      email.value = data.email ?? null
      name.value = data.name ?? null
      sub.value = data.sub ?? null
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 401) {
        isAuthenticated.value = false
        email.value = null
        name.value = null
        sub.value = null
        return
      }

      throw error
    }
  }

  function login() {
    authService.login()
  }

  function logout() {
    authService.logout()
  }

  return { isAuthenticated, email, name, sub, isLoggedIn, init, login, logout }
})
