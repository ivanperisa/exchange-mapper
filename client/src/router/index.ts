import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import { i18n } from '@/i18n'
import CallbackView from '@/views/CallbackView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: () => import('@/views/HomeView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/auth-error',
      component: () => import('@/views/AuthErrorView.vue')
    },
    {
      path: '/logged-out',
      component: () => import('@/views/LoggedOutView.vue')
    },
    {
      path: '/callback',
      component: CallbackView
    }
  ]
})

router.beforeEach(async (to) => {
  const authStore = useAuthStore()
  if (to.path !== '/callback') {
    await authStore.init()
  }

  if (to.meta.requiresAuth && !authStore.isLoggedIn) {
    try {
      await authStore.login(to.fullPath)
    } catch (error) {
      authStore.callbackError = error instanceof Error ? error.message : i18n.global.t('callback.failed')
      return '/auth-error'
    }
    return false
  }
})

export default router
