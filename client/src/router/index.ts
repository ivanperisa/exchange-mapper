import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import LandingView from '@/views/LandingView.vue'
import HomeView from '@/views/HomeView.vue'
import CallbackView from '@/views/CallbackView.vue'
import ExchangeView from '@/views/ExchangeView.vue'
import HistoryView from '@/views/HistoryView.vue'
import OnboardingView from '@/views/OnboardingView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: LandingView
    },
    {
      path: '/home',
      name: 'home',
      component: HomeView,
      meta: { requiresAuth: true }
    },
    {
      path: '/settings',
      name: 'settings',
      component: () => import('@/views/SettingsView.vue'),
      meta: { requiresAuth: true, requiresOnboarding: true }
    },
    {
      path: '/exchange',
      component: ExchangeView,
      meta: { requiresAuth: true }
    },
    {
      path: '/history',
      component: HistoryView,
      meta: { requiresAuth: true }
    },
    {
      path: '/onboarding',
      component: OnboardingView,
      meta: { requiresAuth: true }
    },
    { path: '/callback', component: CallbackView }
  ]
})

router.beforeEach(async (to) => {
  const authStore = useAuthStore()
  await authStore.init()

  if (to.meta.requiresAuth && !authStore.isLoggedIn) {
    return '/'
  }

  if (authStore.isLoggedIn && !authStore.isOnboarded && to.path !== '/onboarding') {
    return '/onboarding'
  }

  if (to.meta.requiresOnboarding && !authStore.isOnboarded) {
    return '/onboarding'
  }

  if (authStore.isLoggedIn && authStore.isOnboarded && to.path === '/onboarding') {
    return '/home'
  }
})

export default router
